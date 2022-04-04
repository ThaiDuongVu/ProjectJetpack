using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    private Player _player;

    public bool IsDashing { get; set; }

    [Header("Dash Properties")]
    [SerializeField] private float dashDistance = 4.5f;
    [SerializeField] private float dashInterpolationRatio = 0.3f;
    [SerializeField] private float dashEpsilon = 0.5f;
    [SerializeField] private int dashRate = 4;
    [SerializeField] private Transform dashPoint;
    [SerializeField] private ParticleSystem muzzlePrefab;
    [SerializeField] private float fuelConsumptionPerDash = 10f;
    [SerializeField] private int healthConsumptionPerDash = 1;

    private Vector2 _dashPosition;
    private Vector2 _dashDirection;
    private Timer _dashTimer;
    private float _dashTimeout;
    private bool _canDash = true;
    private static readonly int IsDashingAnimationTrigger = Animator.StringToHash("isDashing");

    [Header("Combat Properties")]
    [SerializeField] private int damage = 1;
    private Enemy[] TargetEnemies;
    private GameObject[] TargetObstacles;

    [Header("Colors")]
    [SerializeField] private Color grey;
    [SerializeField] private Color transparent;
    [SerializeField] private Color blue;
    [SerializeField] private Color red;

    private InputManager _inputManager;

    #region Input Methods

    private void DashOnStarted(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        // Dash();
        _player.Stagger(Vector2.down);
    }

    #endregion

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle dash input
        _inputManager.Player.Dash.started += DashOnStarted;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    public override void Awake()
    {
        base.Awake();

        _player = GetComponent<Player>();
    }

    public override void Start()
    {
        base.Start();

        _dashTimeout = 1f / dashRate;
        _dashTimer = new Timer(_dashTimeout);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Aim();

        if (IsDashing)
        {
            transform.position = Vector2.Lerp(transform.position, _dashPosition, dashInterpolationRatio);
            if (Vector2.Distance(transform.position, _dashPosition) <= dashEpsilon) SetDash(false);
        }
        if (_dashTimer.IsReached()) _canDash = true;
    }

    #endregion

    private void Aim()
    {
        _player.PlayerArrow.SetColor(blue);
        TargetEnemies = null;

        // Perform raycast to check if any enemies are hit
        var hits = Physics2D.RaycastAll(transform.position, _player.PlayerArrow.CurrentDirection, dashDistance, LayerMask.GetMask("Enemies"));
        if (hits.Length <= 0) return;

        // Set color and targets if raycast hit
        _player.PlayerArrow.SetColor(red);
        for (int i = 0; i < hits.Length; i++) TargetEnemies[i] = hits[i].transform.GetComponent<Enemy>();
    }

    private void DealDamage(Enemy enemy)
    {
        enemy.TakeDamage(damage);
        _player.PlayerCombo.Add(1);
    }

    #region Dash Methods

    private void SetDash(bool value)
    {
        _player.Collider2D.enabled = !value;
        _player.Animator.SetBool(IsDashingAnimationTrigger, value);

        _player.GroundTrail.SetColor(value ? transparent : grey);
        IsDashing = value;
    }

    private void Dash()
    {
        if (IsDashing || !_canDash) return;

        if (_player.PlayerResources.Fuel >= fuelConsumptionPerDash) _player.PlayerResources.Fuel -= fuelConsumptionPerDash;
        else _player.PlayerResources.Health -= healthConsumptionPerDash;

        _dashDirection = _player.PlayerArrow.CurrentDirection;
        _dashPosition = (Vector2)transform.position + _dashDirection * dashDistance;

        SetDash(true);

        Instantiate(muzzlePrefab, dashPoint.position, Quaternion.identity).transform.up = _dashDirection;
        CameraShaker.Instance.Shake(CameraShakeMode.Light);

        _canDash = false;
        _dashTimer.Reset(_dashTimeout);
    }

    #endregion
}