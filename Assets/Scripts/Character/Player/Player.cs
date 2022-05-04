using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : Character
{
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerCombat PlayerCombat { get; private set; }
    public PlayerResources PlayerResources { get; private set; }
    public PlayerCombo PlayerCombo { get; private set; }

    public bool IsControllable { get; set; } = true;

    [Header("Effects Properties")]
    [SerializeField] private Trail groundTrailPrefab;
    public Trail GroundTrail { get; private set; }
    [SerializeField] private Color transparentColor;
    private Color trailDefaultColor;
    [SerializeField] private ParticleSystem bloodSplashPrefab;

    [Header("Crosshair Properties")]
    public SpriteRenderer crosshair;
    public override bool IsGrounded
    {
        get => base.IsGrounded;
        set
        {
            base.IsGrounded = value;
            crosshair.gameObject.SetActive(!value && PlayerResources.Fuel >= PlayerCombat.fuelConsumptionPerJump);
        }
    }

    public bool basePlatformReached;
    public Portal Portal { get; set; }
    public VendingMachine VendingMachine { get; set; }

    [Header("Level Objectives")]
    [SerializeField] private Transform objectiveParent;
    [SerializeField] private Color objectiveIncompleteColor;
    [SerializeField] private Color objectiveCompleteColor;
    private LevelObjective objective1;
    private LevelObjective objective2;
    private LevelObjective objective3;
    [SerializeField] private TMP_Text objective1Text;
    [SerializeField] private TMP_Text objective2Text;
    [SerializeField] private TMP_Text objective3Text;

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

        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerCombat = GetComponent<PlayerCombat>();
        PlayerResources = GetComponent<PlayerResources>();
        PlayerCombo = GetComponent<PlayerCombo>();
    }

    public override void Start()
    {
        base.Start();

        GroundTrail = Instantiate(groundTrailPrefab, transform.position, Quaternion.identity);
        GroundTrail.Target = transform;
        trailDefaultColor = GroundTrail.GetComponent<ParticleSystem>().main.startColor.color;

        if (LevelGenerator.Instance && LevelGenerator.Instance.Variant == LevelVariant.Regular) InitLevelObjectives();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        DetectGrounded();
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -4.5f, 4.5f), transform.position.y);
    }

    #endregion

    #region Damage & Death

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        PlayerCombo.Cancel();
        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
    }

    public override void Die()
    {
        base.Die();

        PlayerResources.ClearTemp();
        GameController.Instance.StartCoroutine(GameController.Instance.GameOver());

        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());

        Destroy(gameObject);
    }

    #endregion

    public override void KnockBack(Vector2 direction, float force)
    {
        base.KnockBack(direction, force);

        Instantiate(bloodSplashPrefab, transform.position, Quaternion.identity).transform.up = direction;
    }

    public override void DetectGrounded()
    {
        base.DetectGrounded();

        GroundTrail.SetColor(IsGrounded ? trailDefaultColor : transparentColor);
        if (GroundPlatform && GroundPlatform.transform.CompareTag("BasePlatform") && !basePlatformReached)
        {
            basePlatformReached = true;
            CheckLevelObjectives();

            CameraShaker.Instance.Shake(CameraShakeMode.Light);
            GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());
        }
    }

    public void EnterPortal()
    {
        if (!Portal) return;

        IsControllable = false;
        PlayerMovement.StopRunningImmediate();
        StartCoroutine(Portal.Enter(this));
    }

    public void PurchaseFromVendingMachine()
    {
        if (!VendingMachine) return;

        VendingMachine.Purchase(this);
    }

    #region Level Objectives Methods

    private void InitLevelObjectives()
    {
        var objective1Set = Resources.LoadAll<LevelObjective>("Levels/Objectives/Set1");
        objective1 = Instantiate(objective1Set[Random.Range(0, objective1Set.Length)], transform.position, Quaternion.identity);
        objective1.transform.parent = objectiveParent;
        objective1Text.text = objective1.name;

        var objective2Set = Resources.LoadAll<LevelObjective>("Levels/Objectives/Set2");
        objective2 = Instantiate(objective2Set[Random.Range(0, objective2Set.Length)], transform.position, Quaternion.identity);
        objective2.transform.parent = objectiveParent;
        objective2Text.text = objective2.name;

        var objective3Set = Resources.LoadAll<LevelObjective>("Levels/Objectives/Set3");
        objective3 = Instantiate(objective3Set[Random.Range(0, objective3Set.Length)], transform.position, Quaternion.identity);
        objective3.transform.parent = objectiveParent;
        objective3Text.text = objective3.name;
    }

    private void UpdateObjectiveTexts()
    {
        objective1Text.color = objective1.IsCompleted ? objectiveCompleteColor : objectiveIncompleteColor;
        objective1Text.fontStyle = objective1.IsCompleted ? FontStyles.Strikethrough : FontStyles.Normal;

        objective2Text.color = objective2.IsCompleted ? objectiveCompleteColor : objectiveIncompleteColor;
        objective2Text.fontStyle = objective2.IsCompleted ? FontStyles.Strikethrough : FontStyles.Normal;

        objective3Text.color = objective3.IsCompleted ? objectiveCompleteColor : objectiveIncompleteColor;
        objective3Text.fontStyle = objective3.IsCompleted ? FontStyles.Strikethrough : FontStyles.Normal;
    }

    private void CheckLevelObjectives()
    {
        if (!objective1 || !objective2 || !objective3) return;

        var basePlatformSpawners = GroundPlatform.GetComponentsInChildren<CollectibleSpawner>();

        if (objective1.IsCompleted) basePlatformSpawners[0].Spawn();
        if (objective2.IsCompleted) basePlatformSpawners[1].Spawn();
        if (objective3.IsCompleted) basePlatformSpawners[2].Spawn();

        if (objective1.IsCompleted && objective2.IsCompleted && objective3.IsCompleted)
            basePlatformSpawners[3].Spawn();

        UpdateObjectiveTexts();
    }

    #endregion
}
