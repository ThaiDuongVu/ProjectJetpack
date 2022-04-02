using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    private Player _player;

    [Header("Aim Properties")]
    [SerializeField] private SpriteRenderer crosshair;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color aimColor;
    private readonly string[] _aimLayers = { "Enemies", "Fireballs" };

    [Header("Jump Properties")]
    [SerializeField] private float jumpForce = 18f;
    [SerializeField] private ParticleSystem jumpMuzzlePrefab;
    [SerializeField] private Transform jumpPoint;
    [SerializeField] private float fuelConsumptionPerJump = 20f;
    [SerializeField] private int healthConsumptionPerJump = 1;
    [SerializeField] private int jumpRate = 4;
    private float _maxJumpTimer;
    private float _jumpTimer;
    private bool _canJump;

    [Header("Hover Properties")]
    [SerializeField] private float hoverForce = 26f;
    [SerializeField] private ParticleSystem hoverMuzzle;
    [SerializeField] private float fuelConsumptionPerHoverSecond = 25f;

    [Header("Recharge Properties")]
    [SerializeField] private float fuelRechargePerSecond = 100f;

    [Header("Shoot Properties")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private BulletEffect bulletEffectPrefab;
    [SerializeField] private ParticleSystem bloodSplashPrefab;

    [Header("Damage Properties")]
    [SerializeField] private int damagePerJump = 1;

    public bool IsInHoverMode { get; set; }

    private InputManager _inputManager;

    #region Input Methods

    private void JumpOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        ExitHoverMode();
        Jump();
    }

    private void HoverOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        EnterHoverMode();
    }

    private void HoverOnCanceled(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        ExitHoverMode();
    }
    #endregion

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle jump input
        _inputManager.Player.Jump.performed += JumpOnPerformed;

        // Handle hover input
        _inputManager.Player.Hover.performed += HoverOnPerformed;
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

        _maxJumpTimer = 1f / jumpRate;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Aim(shootPoint.position, Vector2.down);

        if (_jumpTimer < _maxJumpTimer) _jumpTimer += Time.fixedDeltaTime;
        else _canJump = true;

        if (IsInHoverMode) Hover();
        if (!_player.IsAirbourne) Recharge();
    }

    #endregion

    private void Aim(Vector2 point, Vector2 direction)
    {
        if (!_player.IsAirbourne)
        {
            crosshair.gameObject.SetActive(false);
            return;
        }

        crosshair.gameObject.SetActive(true);
        crosshair.color = normalColor;

        var hit = Physics2D.Raycast(point, direction, Mathf.Infinity, LayerMask.GetMask(_aimLayers));
        if (!hit) return;

        crosshair.color = aimColor;
    }

    #region Jump & Hover

    private void Jump()
    {
        if (!_canJump) return;

        _player.Rigidbody2D.velocity = Vector2.zero;
        _player.Rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        CameraShaker.Instance.Shake(Shoot(shootPoint.position, Vector2.down) ? CameraShakeMode.Normal : CameraShakeMode.Light);

        _jumpTimer = 0f;
        _canJump = false;
        if (_player.PlayerResources.Fuel < fuelConsumptionPerJump) _player.PlayerResources.Health -= healthConsumptionPerJump;
        else _player.PlayerResources.Fuel -= fuelConsumptionPerJump;

        Instantiate(jumpMuzzlePrefab, jumpPoint.position, Quaternion.identity);
    }

    private void EnterHoverMode()
    {
        if (_player.PlayerResources.Fuel <= 0f) return;

        _player.Rigidbody2D.velocity = Vector2.zero;
        hoverMuzzle.Play();

        IsInHoverMode = true;
    }

    public void ExitHoverMode()
    {
        hoverMuzzle.Stop();

        IsInHoverMode = false;
    }

    private void Hover()
    {
        if (_player.PlayerResources.Fuel <= 0f) ExitHoverMode();
        _player.Rigidbody2D.AddForce(Vector2.up * hoverForce, ForceMode2D.Force);
        _player.PlayerResources.Fuel -= fuelConsumptionPerHoverSecond * Time.fixedDeltaTime;

        CameraShaker.Instance.Shake(CameraShakeMode.Nano);
    }

    #endregion

    private void Recharge()
    {
        _player.PlayerResources.Fuel += fuelRechargePerSecond * Time.fixedDeltaTime;
    }

    private bool Shoot(Vector2 point, Vector2 direction)
    {
        var hit = Physics2D.Raycast(point, direction, Mathf.Infinity);
        if (!hit) return false;

        if (hit.transform.CompareTag("Enemy"))
        {
            var enemy = hit.transform.GetComponent<Enemy>();
            enemy.TakeDamage(damagePerJump);

            _player.PlayerCombo.Add(1);
            Instantiate(bloodSplashPrefab, hit.point, Quaternion.identity).transform.up = -direction;
            Instantiate(bulletEffectPrefab, shootPoint.position, Quaternion.identity).targetPosition = hit.point;
        }
        else if (hit.transform.CompareTag("Fireball"))
        {
            var fireball = hit.transform.GetComponent<Fireball>();
            fireball.ReturnToSender();

            _player.PlayerCombo.Add(1);
            Instantiate(bulletEffectPrefab, shootPoint.position, Quaternion.identity).targetPosition = hit.point;
        }

        return true;
    }
}