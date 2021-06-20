using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private new Camera camera;

    private Vector2 movement;
    private Vector2 currentDirection;
    private float currentVelocity;

    public float MaxVelocity { get; set; } = 30f;
    public float MinVelocity { get; set; } = 0f;
    public float Acceleration { get; set; } = 60f;
    public float Deceleration { get; set; } = 60f;

    private static readonly int IsRunningAnimationTrigger = Animator.StringToHash("isRunning");
    private static readonly int EnterDashAnimationTrigger = Animator.StringToHash("enterDash");
    private static readonly int ExitDashAnimationTrigger = Animator.StringToHash("exitDash");

    private float LookInterpolationRatio { get; set; } = 0.3f;

    private InputManager inputManager;

    /// <summary>
    /// Unity Event function.
    /// Initialize input handler on object enabled.
    /// </summary>
    private void OnEnable()
    {
        inputManager = new InputManager();

        // Handle movement input
        inputManager.Player.Move.performed += MoveOnPerformed;
        inputManager.Player.Move.canceled += MoveOnCanceled;

        inputManager.Enable();
    }

    #region Input Methods

    /// <summary>
    /// On move input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void MoveOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance.State != GameState.Started || player.IsStaggered) return;

        // Set movement vector
        currentDirection = context.ReadValue<Vector2>();
        StartRunning();
    }

    /// <summary>
    /// On move input canceled.
    /// </summary>
    /// <param name="context">Input context</param>
    private void MoveOnCanceled(InputAction.CallbackContext context)
    {
        // Update player state
        player.IsRunning = false;
    }

    #endregion

    /// <summary>
    /// Unity Event function.
    /// Disable input handling on object disabled.
    /// </summary>
    private void OnDisable()
    {
        inputManager.Disable();
    }

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        player = Player.Instance;
        camera = Camera.main;
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;

        // If current direction is not empty then accelerate
        if (player.IsRunning) Accelerate();
        // If not then decelerate
        else Decelerate();

        // Move player at current velocity
        if (!player.IsStaggered) Run();
        // Sync player animation with current velocity
        if (!player.IsStaggered) Animate();
    }

    /// <summary>
    /// Player start running.
    /// </summary>
    private void StartRunning()
    {
        // Play run animation
        player.Animator.SetBool(IsRunningAnimationTrigger, true);
        // Update player state
        player.IsRunning = true;
    }

    /// <summary>
    /// Player stop running.
    /// </summary>
    private void StopRunning()
    {
        // Reset current direction
        currentDirection = Vector2.zero;
        // Stop run animation
        player.Animator.SetBool(IsRunningAnimationTrigger, false);
    }

    /// <summary>
    /// Accelerate if current velocity is less than max velocity.
    /// </summary>
    private void Accelerate()
    {
        if (currentVelocity < MaxVelocity) currentVelocity += Acceleration * Time.deltaTime;
        else if (currentVelocity > MaxVelocity) currentVelocity = MaxVelocity;
    }

    /// <summary>
    /// Decelerate if current velocity is greater than min velocity.
    /// </summary>
    private void Decelerate()
    {
        if (currentVelocity > MinVelocity) currentVelocity -= Deceleration * Time.deltaTime;
        // If player near stopping then stop
        else StopRunning();
    }

    /// <summary>
    /// Move player to movement vector.
    /// </summary>
    private void Run()
    {
        // Movement vector derived from input direction and camera angle
        movement = Quaternion.Euler(0f, 0f, camera.transform.eulerAngles.z) * currentDirection;
        // Move player
        player.Rigidbody2D.MovePosition(
            player.Rigidbody2D.position + movement * (currentVelocity * Time.fixedDeltaTime));
        // Rotate player to movement direction
        // player.transform.up = Vector2.Lerp(player.transform.up, movement, LookInterpolationRatio);
        player.transform.up = Vector2.Lerp(player.transform.up, camera.transform.up, LookInterpolationRatio);
    }

    /// <summary>
    /// Scale animation speed to movement speed.
    /// </summary>
    private void Animate()
    {
        // Set animation speed to velocity length if sync animation is enabled
        if (currentVelocity > 0f) player.Animator.speed = currentVelocity / MaxVelocity;
        else player.Animator.speed = 1f;
    }
}