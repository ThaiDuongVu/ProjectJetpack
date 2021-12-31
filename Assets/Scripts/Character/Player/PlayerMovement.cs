using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : CharacterMovement
{
    public Player Player { get; private set; }

    [SerializeField] private int maxJumps = 2;
    private int jumps;

    [SerializeField] private ParticleSystem jumpParticlePrefab;

    private InputManager inputManager;

    /// <summary>
    /// Unity Event function.
    /// On current object enabled.
    /// </summary>
    private void OnEnable()
    {
        inputManager = new InputManager();

        // Handle move input
        inputManager.Player.Move.performed += MoveOnPerformed;
        inputManager.Player.Move.canceled += MoveOnCanceled;

        // Handle jump input
        // inputManager.Player.Jump.performed += JumpOnPerformed;

        inputManager.Enable();
    }

    /// <summary>
    /// Unity Event function.
    /// On current object disabled.
    /// </summary>
    private void OnDisable()
    {
        inputManager.Disable();
    }

    #region Input Methods

    /// <summary>
    /// On move input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void MoveOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused || !Player.IsControllable || Player.IsStagger) return;
        InputTypeController.Instance.CheckInputType(context);

        StartRunning(context.ReadValue<Vector2>());
    }

    /// <summary>
    /// On move input canceled.
    /// </summary>
    /// <param name="context">Input context</param>
    private void MoveOnCanceled(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused || !Player.IsControllable || Player.IsStagger) return;
        InputTypeController.Instance.CheckInputType(context);

        StopRunning();
    }

    /// <summary>
    /// On jump input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void JumpOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused || !Player.IsControllable || Player.IsStagger) return;
        if (Player.Jetpack.IsHovering) return;
        if (!IsGrounded && jumps <= 0) return;
        InputTypeController.Instance.CheckInputType(context);

        Jump();
    }

    #endregion

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    public override void Awake()
    {
        base.Awake();

        Player = GetComponent<Player>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    public override void Start()
    {
        base.Start();

        jumps = maxJumps;
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsGrounded && jumps < maxJumps && Player.Rigidbody2D.velocity.y <= 0f) jumps = maxJumps;
    }

    /// <summary>
    /// Apply force upward to jump player.
    /// </summary>
    public override void Jump()
    {
        base.Jump();

        jumps--;
        Instantiate(jumpParticlePrefab, transform.position, Quaternion.identity);
    }
}
