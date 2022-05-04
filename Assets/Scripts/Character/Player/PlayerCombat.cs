using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    private Player _player;

    [Header("Jump Properties")]
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private Transform jumpPoint;
    [SerializeField] private ParticleSystem jumpMuzzlePrefab;
    [SerializeField] private int jumpRate = 4;
    private Timer _jumpTimer;
    private float _jumpTimeout;
    private bool _canJump = true;

    [Header("Hover Properties")]
    [SerializeField] private float hoverForce = 26f;
    [SerializeField] private ParticleSystem hoverMuzzle;
    private bool _isHovering;

    [Header("Combat Properties")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float range = 5f;
    [SerializeField] private ParticleSystem bloodSplashPrefab;

    [Header("Crosshair Properties")]
    [SerializeField] private SpriteRenderer crosshair;
    [SerializeField] private Color regularColor;
    [SerializeField] private Color aimColor;

    [Header("Fuel Properties")]
    public float fuelConsumptionPerJump = 0.1f;
    [SerializeField] private float fuelConsumptionPerHoverSecond = 0.25f;
    [SerializeField] private float fuelRechargeRate = 0.5f;

    private Enemy _targetEnemy;
    private Vector2 _hitPoint;
    [SerializeField] private BulletEffect bulletEffectPrefab;

    private DestructablePlatform _targetDestructablePlatform;

    private InputManager _inputManager;

    #region Input Methods

    private void JumpOnStarted(InputAction.CallbackContext context)
    {
        if (!_player.IsControllable || GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        if (_isHovering) SetHovering(false);
        Jump();
    }

    private void HoverOnStarted(InputAction.CallbackContext context)
    {
        if (!_player.IsControllable || GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        SetHovering(true);
    }

    private void HoverOnCanceled(InputAction.CallbackContext context)
    {
        if (!_player.IsControllable || GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        SetHovering(false);
    }

    #endregion

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle jump input
        _inputManager.Player.Jump.started += JumpOnStarted;

        // Handle hover input
        _inputManager.Player.Hover.started += HoverOnStarted;
        _inputManager.Player.Hover.canceled += HoverOnCanceled;

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

        _jumpTimeout = 1f / jumpRate;
        _jumpTimer = new Timer(_jumpTimeout);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Aim();

        if (_isHovering) Hover();
        if (_jumpTimer.IsReached()) _canJump = true;
        if (_player.IsGrounded) _player.PlayerResources.Fuel += fuelRechargeRate * Time.fixedDeltaTime;
    }

    #endregion

    private void Aim()
    {
        _targetEnemy = null;
        _targetDestructablePlatform = null;
        crosshair.color = regularColor;

        var hit = Physics2D.Raycast(transform.position, Vector2.down, range);
        if (!hit) return;

        var isHit = false;
        
        if (hit.transform.CompareTag("Enemy"))
        {
            _targetEnemy = hit.transform.GetComponent<Enemy>();
            isHit = true;
        }
        else if (hit.transform.CompareTag("DestructablePlatform"))
        {
            _targetDestructablePlatform = hit.transform.GetComponent<DestructablePlatform>();
            isHit = true;
        }

        if (!isHit) return;

        _hitPoint = hit.point;
        crosshair.color = aimColor;
    }

    private void Jump()
    {
        if (!_canJump || _player.PlayerResources.Fuel < fuelConsumptionPerJump) return;

        _player.Rigidbody2D.velocity = Vector2.zero;
        _player.Rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        _player.PlayerResources.Fuel -= fuelConsumptionPerJump;

        Instantiate(jumpMuzzlePrefab, jumpPoint.position, Quaternion.identity);

        _canJump = false;
        _jumpTimer.Reset(_jumpTimeout);

        Fire();
    }

    #region Hover Methods

    private void SetHovering(bool value)
    {
        if (_player.PlayerResources.Fuel <= 0f) return;

        _player.Rigidbody2D.velocity = Vector2.zero;

        if (value) hoverMuzzle.Play();
        else hoverMuzzle.Stop();

        _isHovering = value;
    }

    public void StopHovering()
    {
        SetHovering(false);
    }

    private void Hover()
    {
        if (_player.PlayerResources.Fuel <= 0f)
        {
            SetHovering(false);
            return;
        }

        _player.Rigidbody2D.AddForce(Vector2.up * hoverForce, ForceMode2D.Force);
        _player.PlayerResources.Fuel -= fuelConsumptionPerHoverSecond * Time.fixedDeltaTime;

        CameraShaker.Instance.Shake(CameraShakeMode.Nano);
    }

    #endregion

    private void Fire()
    {
        if (_targetEnemy)
        {
            _targetEnemy.TakeDamage(damage);
            Instantiate(bloodSplashPrefab, _hitPoint, Quaternion.identity);
            Instantiate(bulletEffectPrefab, transform.position, Quaternion.identity).TargetPosition = _hitPoint;

            CameraShaker.Instance.Shake(CameraShakeMode.Normal);
            _player.PlayerCombo.Add();
        }
        else if (_targetDestructablePlatform)
        {
            _targetDestructablePlatform.Explode();
            Instantiate(bulletEffectPrefab, transform.position, Quaternion.identity).TargetPosition = _hitPoint;

            CameraShaker.Instance.Shake(CameraShakeMode.Normal);
        }
        else CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }
}
