using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : Character
{
    private PlayerMovement _playerMovement;
    private PlayerCombat _playerCombat;
    public PlayerResources PlayerResources { get; private set; }
    public PlayerCombo PlayerCombo { get; private set; }

    public bool IsControllable { get; set; } = true;

    [Header("Effects Properties")]
    [SerializeField] private Trail groundTrailPrefab;
    private Trail _groundTrail;
    [SerializeField] private Color transparentColor;
    private Color _trailDefaultColor;
    [SerializeField] private ParticleSystem bloodSplashPrefab;

    [Header("Crosshair Properties")]
    public SpriteRenderer crosshair;
    public override bool IsGrounded
    {
        get => base.IsGrounded;
        protected set
        {
            base.IsGrounded = value;
            crosshair.gameObject.SetActive(!value && PlayerResources.Fuel >= _playerCombat.fuelConsumptionPerJump);
        }
    }

    public bool basePlatformReached;
    public Portal Portal { get; set; }
    public VendingMachine VendingMachine { get; set; }

    [Header("Level Objectives")]
    [SerializeField] private Transform objectiveParent;
    [SerializeField] private Color objectiveIncompleteColor;
    [SerializeField] private Color objectiveCompleteColor;
    [SerializeField] private TMP_Text objective1Text;
    [SerializeField] private TMP_Text objective2Text;
    [SerializeField] private TMP_Text objective3Text;
    private LevelObjective _objective1;
    private LevelObjective _objective2;
    private LevelObjective _objective3;

    public const string TokensKey = "Tokens";
    public const string CustomizationUnlockKey = "CustomizationUnlock";
    public const string CustomizationEquipKey = "CustomizationEquip";

    private InputManager _inputManager;

    #region Input Methods

    private void InteractOnPerformed(InputAction.CallbackContext context)
    {
        if (!IsControllable || GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        EnterPortal();
        PurchaseFromVendingMachine();
    }

    #endregion

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle interact input
        _inputManager.Player.Interact.performed += InteractOnPerformed;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    public override void Awake()
    {
        base.Awake();

        _playerMovement = GetComponent<PlayerMovement>();
        _playerCombat = GetComponent<PlayerCombat>();
        PlayerResources = GetComponent<PlayerResources>();
        PlayerCombo = GetComponent<PlayerCombo>();

        PlayerPrefs.SetInt($"{Player.CustomizationUnlockKey}0", 1);
    }

    public override void Start()
    {
        base.Start();

        _groundTrail = Instantiate(groundTrailPrefab, transform.position, Quaternion.identity);
        _groundTrail.Target = transform;
        _trailDefaultColor = _groundTrail.GetComponent<ParticleSystem>().main.startColor.color;

        if (LevelGenerator.Instance && LevelGenerator.Instance.Variant == LevelVariant.Regular) InitLevelObjectives();

        LoadCustomization();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        DetectGrounded();
        var position = transform.position;
        position = new Vector2(Mathf.Clamp(position.x, -4.5f, 4.5f), position.y);
        transform.position = position;
    }

    #endregion

    #region Damage & Death

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        PlayerCombo.Cancel();
        AudioController.Instance.Play(AudioVariant.PlayerTakeDamage);
        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
    }

    public override void Die()
    {
        base.Die();

        PlayerPrefs.SetInt(TokensKey, PlayerPrefs.GetInt(TokensKey, 0) + PlayerResources.Token);
        PlayerResources.ClearTemp();

        GameController.Instance.StartCoroutine(GameController.Instance.GameOver());
        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
        AudioController.Instance.Play(AudioVariant.PlayerExplode);
        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());

        Destroy(gameObject);
    }

    #endregion

    public override void KnockBack(Vector2 direction, float force)
    {
        base.KnockBack(direction, force);

        Instantiate(bloodSplashPrefab, transform.position, Quaternion.identity).transform.up = direction;
    }

    protected override void DetectGrounded()
    {
        base.DetectGrounded();

        _groundTrail.SetColor(IsGrounded ? _trailDefaultColor : transparentColor);
        if (!GroundPlatform || !GroundPlatform.transform.CompareTag("BasePlatform") || basePlatformReached || !LevelGenerator.Instance || LevelGenerator.Instance.Variant != LevelVariant.Regular) return;

        basePlatformReached = true;
        CheckLevelObjectives();

        AudioController.Instance.Play(AudioVariant.PlayerReachBasePlatform);
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());
    }

    private void EnterPortal()
    {
        if (!Portal) return;

        IsControllable = false;
        _playerMovement.StopRunningImmediate();
        StartCoroutine(Portal.Enter(this));

        AudioController.Instance.Play(AudioVariant.PlayerEnterPortal);
    }

    private void PurchaseFromVendingMachine()
    {
        if (!VendingMachine) return;

        VendingMachine.Purchase(this);
        LoadCustomization();
        foreach (var vendingMachine in FindObjectsOfType<VendingMachineCustomization>()) vendingMachine.UpdateText();
    }

    private void LoadCustomization()
    {
        var customizations = Resources.LoadAll<UnlockableCustomization>("Customization");
        var equipId = PlayerPrefs.GetInt(CustomizationEquipKey, 0);

        foreach (var customization in customizations)
            if (customization.id == equipId) Animator.runtimeAnimatorController = customization.animatorController;

    }

    #region Level Objectives Methods

    private void InitLevelObjectives()
    {
        var objective1Set = Resources.LoadAll<LevelObjective>("Levels/Objectives/Set1");
        var position = transform.position;
        _objective1 = Instantiate(objective1Set[Random.Range(0, objective1Set.Length)], position, Quaternion.identity);
        _objective1.transform.parent = objectiveParent;
        objective1Text.text = _objective1.name;

        var objective2Set = Resources.LoadAll<LevelObjective>("Levels/Objectives/Set2");
        _objective2 = Instantiate(objective2Set[Random.Range(0, objective2Set.Length)], position, Quaternion.identity);
        _objective2.transform.parent = objectiveParent;
        objective2Text.text = _objective2.name;

        var objective3Set = Resources.LoadAll<LevelObjective>("Levels/Objectives/Set3");
        _objective3 = Instantiate(objective3Set[Random.Range(0, objective3Set.Length)], position, Quaternion.identity);
        _objective3.transform.parent = objectiveParent;
        objective3Text.text = _objective3.name;
    }

    private void UpdateObjectiveTexts()
    {
        objective1Text.color = _objective1.IsCompleted ? objectiveCompleteColor : objectiveIncompleteColor;
        objective1Text.fontStyle = _objective1.IsCompleted ? FontStyles.Strikethrough : FontStyles.Normal;

        objective2Text.color = _objective2.IsCompleted ? objectiveCompleteColor : objectiveIncompleteColor;
        objective2Text.fontStyle = _objective2.IsCompleted ? FontStyles.Strikethrough : FontStyles.Normal;

        objective3Text.color = _objective3.IsCompleted ? objectiveCompleteColor : objectiveIncompleteColor;
        objective3Text.fontStyle = _objective3.IsCompleted ? FontStyles.Strikethrough : FontStyles.Normal;
    }

    private void CheckLevelObjectives()
    {
        if (!_objective1 || !_objective2 || !_objective3) return;

        var basePlatformSpawners = GroundPlatform.GetComponentsInChildren<CollectibleSpawner>();

        if (_objective1.IsCompleted) basePlatformSpawners[0].Spawn();
        if (_objective2.IsCompleted) basePlatformSpawners[1].Spawn();
        if (_objective3.IsCompleted) basePlatformSpawners[2].Spawn();

        if (_objective1.IsCompleted && _objective2.IsCompleted && _objective3.IsCompleted)
            basePlatformSpawners[3].Spawn();

        UpdateObjectiveTexts();
    }

    #endregion
}
