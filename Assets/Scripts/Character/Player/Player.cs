using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : Character
{
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerCombat PlayerCombat { get; private set; }
    public PlayerResources PlayerResources { get; private set; }
    public PlayerCombo PlayerCombo { get; private set; }
    public PlayerArrow PlayerArrow { get; private set; }

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

    public bool BasePlatformReached { get; set; }
    public Portal Portal { get; set; }

    [Header("Level Objectives")]
    [SerializeField] private Transform objectiveParent;
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
        if (GameController.Instance.State == GameState.Paused || !IsControllable) return;
        InputTypeController.Instance.CheckInputType(context);

        if (!Portal) return;

        CheckLevelObjectives();
        StartCoroutine(Portal.Enter(this));
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
        PlayerArrow = GetComponentInChildren<PlayerArrow>();

        LoadLevelObjectives();
    }

    public override void Start()
    {
        base.Start();

        GroundTrail = Instantiate(groundTrailPrefab, transform.position, Quaternion.identity);
        GroundTrail.Target = transform;
        trailDefaultColor = GroundTrail.GetComponent<ParticleSystem>().main.startColor.color;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        DetectGrounded();
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

        GameController.Instance.StartCoroutine(GameController.Instance.GameOver());

        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
        GameController.Instance.StartCoroutine(GameController.Instance.SlowDownEffect());

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
        if (GroundPlatform.transform.CompareTag("BasePlatform") && !BasePlatformReached) BasePlatformReached = true;
    }

    #region Level Objectives Methods

    private void LoadLevelObjectives()
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

    private void CheckLevelObjectives()
    {
        // TODO: Reward player for completing level objectives
    }

    #endregion
}
