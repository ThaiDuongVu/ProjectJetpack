using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    private Player _player;

    [Header("Jump Properties")]
    [SerializeField] private float jumpForce = 18f;
    [SerializeField] private ParticleSystem jumpMuzzlePrefab;
    [SerializeField] private Transform jumpPoint;
    [SerializeField] private float fuelConsumptionPerJump = 20f;
    [SerializeField] private int jumpRate = 4;
    private float _maxJumpTimer;
    private float _jumpTimer;
    private bool _canJump;

    [Header("Hover Properties")]
    [SerializeField] private float hoverForce = 26f;
    [SerializeField] private ParticleSystem hoverMuzzle;
    [SerializeField] private float fuelConsumptionPerHoverSecond = 25f;

    [Header("Recharge Properties")]
    [SerializeField] private float fuelRechargePerSecond = 50f;

    [Header("Shoot Properties")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject shootLine;
    [SerializeField] private BulletEffect bulletEffectPrefab;
    [SerializeField] private ParticleSystem bloodSplashPrefab;

    [Header("Damage Properties")]
    [SerializeField] private int damagerPerJump = 1;
    [SerializeField] private int damagePerHoverSecond = 1;

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

        if (_jumpTimer < _maxJumpTimer) _jumpTimer += Time.fixedDeltaTime;
        else _canJump = true;

        if (IsInHoverMode) Hover();
        if (!_player.IsAirbourne) Recharge();
    }

    #endregion

    #region Jump & Hover

    private void Jump()
    {
        if (!_canJump) return;
        if (_player.PlayerResources.Fuel < fuelConsumptionPerJump) return;

        _player.Rigidbody2D.velocity = Vector2.zero;
        _player.Rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        JumpShoot(shootPoint.position, Vector2.down);

        _jumpTimer = 0f;
        _canJump = false;
        _player.PlayerResources.Fuel -= fuelConsumptionPerJump;

        Instantiate(jumpMuzzlePrefab, jumpPoint.position, Quaternion.identity);
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    private void EnterHoverMode()
    {
        if (_player.PlayerResources.Fuel <= 0f) return;

        _player.Rigidbody2D.velocity = Vector2.zero;
        hoverMuzzle.Play();

        IsInHoverMode = true;
    }

    private void ExitHoverMode()
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

    private void JumpShoot(Vector2 point, Vector2 direction)
    {
        var hit = Physics2D.Raycast(point, direction, Mathf.Infinity, LayerMask.GetMask("Enemies"));
        if (!hit) return;

        var enemy = hit.transform.GetComponent<Enemy>();
        enemy.TakeDamage(damagerPerJump);

        Instantiate(bulletEffectPrefab, shootPoint.position, Quaternion.identity).targetPosition = hit.point;
        Instantiate(bloodSplashPrefab, hit.point, Quaternion.identity).transform.up = -direction;
        // StartCoroutine(FlashShootLine(hit.point));
    }

    private IEnumerator FlashShootLine(Vector2 point)
    {
        shootLine.transform.localScale = new Vector2(0.125f, shootPoint.position.y - point.y);
        shootLine.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        shootLine.SetActive(false);
    }
}