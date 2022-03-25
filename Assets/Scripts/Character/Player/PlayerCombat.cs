using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    private Player _player;

    [Header("Jump Properties")]
    [SerializeField] private float jumpForce = 18f;
    [SerializeField] private ParticleSystem jumpMuzzlePrefab;
    [SerializeField] private Transform jumpPoint;

    [Header("Hover Properties")]
    [SerializeField] private float hoverForce = 26f;
    [SerializeField] private ParticleSystem hoverMuzzle;

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
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsInHoverMode) Hover();
    }

    #endregion

    private void Jump()
    {
        _player.Rigidbody2D.velocity = Vector2.zero;
        _player.Rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        Instantiate(jumpMuzzlePrefab, jumpPoint.position, Quaternion.identity);
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    private void EnterHoverMode()
    {
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
        _player.Rigidbody2D.AddForce(Vector2.up * hoverForce, ForceMode2D.Force);

        CameraShaker.Instance.Shake(CameraShakeMode.Nano);
    }
}