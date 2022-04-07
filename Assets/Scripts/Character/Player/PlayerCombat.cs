using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    private Player _player;

    public bool IsDashing { get; set; }

    [Header("Dash Properties")]
    [SerializeField] private float dashDistance = 4f;
    [SerializeField] private float dashInterpolationRatio = 0.2f;
    [SerializeField] private float dashEpsilon = 0.5f;
    [SerializeField] private int dashRate = 4;
    [SerializeField] private Transform dashPoint;
    [SerializeField] private ParticleSystem muzzlePrefab;
    [SerializeField] private TrailRenderer dashTrailRedPrefab;
    [SerializeField] private TrailRenderer dashTrailBluePrefab;
    private TrailRenderer _dashTrail;
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
    private Enemy[] _targetEnemies;
    private Vector2 _obstacleHitPoint;
    private readonly Vector2 ObstacleDefaultPoint = new Vector2(100f, 100f);
    private string[] _obstacleLayers = { "Obstacles", "Levels" };

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

        Dash();
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
        _targetEnemies = null;

        // Perform raycast to check if any obstacles are hit
        var obstacleHit = Physics2D.Raycast(dashPoint.position, _player.PlayerArrow.CurrentDirection, dashDistance, LayerMask.GetMask(_obstacleLayers));
        _obstacleHitPoint = obstacleHit ? obstacleHit.point : ObstacleDefaultPoint;
        if (obstacleHit) _player.PlayerArrow.SetColor(grey);

        // Perform raycast to check if any enemies are hit
        var enemyHits = Physics2D.RaycastAll(dashPoint.position, _player.PlayerArrow.CurrentDirection, dashDistance, LayerMask.GetMask("EnemyHitBoxes"));
        if (enemyHits is not { Length: > 0 }) return;

        // Set target enemies based on raycast results    
        _targetEnemies = new Enemy[enemyHits.Length];
        for (var i = 0; i < enemyHits.Length; i++)
        {
            _targetEnemies[i] = enemyHits[i].transform.GetComponent<Enemy>();
            
            // Remove an enemy from target list if it is obstructed behind an obstacle
            if (_obstacleHitPoint != ObstacleDefaultPoint)
            {
                if (Vector2.Distance(enemyHits[i].transform.position, dashPoint.position) > Vector2.Distance(_obstacleHitPoint, dashPoint.position))
                    _targetEnemies[i] = null;
            }
        }

        // Set arrow color based on raycast results
        foreach (var enemy in _targetEnemies)
        {
            if (enemy) _player.PlayerArrow.SetColor(red);
            break;
        }
    }

    private void DealDamage(Enemy enemy)
    {
        if (!enemy) return;

        enemy.TakeDamage(damage);
        _player.PlayerCombo.Add(1);
    }

    #region Dash Methods

    private void SetDash(bool value)
    {
        _player.Collider2D.enabled = !value;
        _player.Animator.SetBool(IsDashingAnimationTrigger, value);

        _player.GroundTrail.SetColor(value ? transparent : grey);
        if (value)
        {
            if (_targetEnemies != null)
            {
                _dashTrail = Instantiate(dashTrailRedPrefab, transform.position, Quaternion.identity);
                _dashTrail.transform.parent = transform;
            }
        }
        else
        {
            if (_dashTrail) _dashTrail.transform.parent = null;
        }

        IsDashing = value;
    }

    private void Dash()
    {
        if (IsDashing || !_canDash) return;

        if (_player.PlayerResources.Fuel >= fuelConsumptionPerDash) _player.PlayerResources.Fuel -= fuelConsumptionPerDash;
        else _player.PlayerResources.Health -= healthConsumptionPerDash;

        _dashDirection = _player.PlayerArrow.CurrentDirection;
        _dashPosition = _obstacleHitPoint == ObstacleDefaultPoint ? (Vector2)transform.position + _dashDirection * dashDistance : _obstacleHitPoint;

        SetDash(true);
        if (_targetEnemies != null) foreach (var enemy in _targetEnemies) DealDamage(enemy);

        Instantiate(muzzlePrefab, dashPoint.position, Quaternion.identity).transform.up = _dashDirection;
        CameraShaker.Instance.Shake(CameraShakeMode.Light);

        _canDash = false;
        _dashTimer.Reset(_dashTimeout);
    }

    #endregion
}