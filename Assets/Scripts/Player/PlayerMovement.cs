using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private new Camera camera;

    private Vector2 movement;
    private Vector2 currentDirection;
    private Vector2 tempDirection;
    private float currentVelocity;

    public float MaxRunningVelocity { get; set; } = 25f;
    public float MaxFlyingVelocity { get; set; } = 50f;
    public float CurrentMaxVelocity { get; set; }
    public float CurrentMinVelocity { get; set; } = 0f;
    public float Acceleration { get; set; } = 80f;
    public float Deceleration { get; set; } = 100f;

    private static readonly int IsRunningAnimationTrigger = Animator.StringToHash("isRunning");
    private static readonly int IsFlyingAnimationTrigger = Animator.StringToHash("isFlying");

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

        // Handle bullet input
        inputManager.Player.Fly.performed += FlyOnPerformed;
        inputManager.Player.Fly.canceled += FlyOnCanceled;

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
        if (GameController.Instance.State != GameState.Started || player.State == PlayerState.Stagger || player.State == PlayerState.Flying) return;

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
        if (GameController.Instance.State != GameState.Started || player.State == PlayerState.Stagger || player.State == PlayerState.Flying) return;

        // Reset movement vector
        currentDirection = Vector2.zero;
    }

    /// <summary>
    /// On bullet mode input started.
    /// </summary>
    /// <param name="context">Input context</param>
    private void FlyOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance.State != GameState.Started) return;

        // Set movement vector
        currentDirection = transform.up;
        StartFlying();
    }

    /// <summary>
    /// On bullet mode input canceled.
    /// </summary>
    /// <param name="context">Input context</param>
    private void FlyOnCanceled(InputAction.CallbackContext context)
    {
        // Reset movement vector
        currentDirection = Vector2.zero;
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
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        CurrentMaxVelocity = MaxRunningVelocity;
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;

        // Move player at current velocity
        if (player.State == PlayerState.Running) Run();
        else if (player.State == PlayerState.Flying) Fly();
        // Sync player animation with current velocity
        Animate();
    }

    /// <summary>
    /// Player start running.
    /// </summary>
    private void StartRunning()
    {
        // Play run animation
        player.Animator.SetBool(IsRunningAnimationTrigger, true);
        // Update player state
        player.State = PlayerState.Running;
    }

    /// <summary>
    /// Player stop running.
    /// </summary>
    private void StopRunning()
    {
        // Stop run animation
        player.Animator.SetBool(IsRunningAnimationTrigger, false);
        // Update player state
        player.State = PlayerState.Idle;
    }

    /// <summary>
    /// Player start flying.
    /// </summary>
    private void StartFlying()
    {
        // Play fly animation
        player.Animator.SetBool(IsFlyingAnimationTrigger, true);
        // Update player state & max velocity
        player.State = PlayerState.Flying;
        CurrentMaxVelocity = MaxFlyingVelocity;
    }

    /// <summary>
    /// Player stop flying.
    /// </summary>
    private void StopFlying()
    {
        // Stop fly animation
        player.Animator.SetBool(IsFlyingAnimationTrigger, false);
        // Update player state & max velocity
        player.State = PlayerState.Idle;
        CurrentMaxVelocity = MaxRunningVelocity;
    }

    /// <summary>
    /// Accelerate if current velocity is less than max velocity.
    /// </summary>
    private void Accelerate()
    {
        if (currentVelocity < CurrentMaxVelocity) currentVelocity += Acceleration * Time.deltaTime;
        else if (currentVelocity > CurrentMaxVelocity) currentVelocity -= Deceleration * Time.deltaTime;
    }

    /// <summary>
    /// Decelerate if current velocity is greater than min velocity.
    /// </summary>
    private void Decelerate()
    {
        if (currentVelocity > CurrentMinVelocity) currentVelocity -= Deceleration * Time.deltaTime;
        else if (currentVelocity < CurrentMinVelocity)
        {
            currentVelocity = CurrentMinVelocity;
            // Stop running/flying if velocity reaches 0
            StopRunning();
            StopFlying();
        }
    }

    /// <summary>
    /// Move player to movement vector.
    /// </summary>
    private void Run()
    {
        // Accelerate/decelerate according to current direction
        if (currentDirection != Vector2.zero)
        {
            Accelerate();
            tempDirection = currentDirection;
        }
        else
        {
            Decelerate();
        }
        // Movement vector derived from input direction and camera angle
        movement = Quaternion.Euler(0f, 0f, camera.transform.eulerAngles.z) * tempDirection;

        // Move player
        player.Rigidbody2D.MovePosition(player.Rigidbody2D.position + movement * (currentVelocity * Time.fixedDeltaTime));
        // Rotate player to movement direction
        player.transform.up = Vector2.Lerp(player.transform.up, camera.transform.up, LookInterpolationRatio);
    }

    /// <summary>
    /// Move player forward at a higher speed.
    /// </summary>
    private void Fly()
    {
        // Accelerate/decelerate according to current direction
        if (currentDirection != Vector2.zero) Accelerate();
        else Decelerate();

        // Movement vector derived from input direction and camera angle
        movement = camera.transform.up;

        // Move player
        player.Rigidbody2D.MovePosition(player.Rigidbody2D.position + movement * (currentVelocity * Time.fixedDeltaTime));
        // Rotate player to movement direction
        player.transform.up = Vector2.Lerp(player.transform.up, camera.transform.up, LookInterpolationRatio);
    }

    /// <summary>
    /// Scale animation speed to movement speed.
    /// </summary>
    private void Animate()
    {
        // If player is not running then default animation speed
        if (player.State != PlayerState.Running)
        {
            player.Animator.speed = 1f;
            return;
        }

        // Set animation speed to velocity length if sync animation is enabled
        if (currentVelocity > 0f) player.Animator.speed = currentVelocity / CurrentMaxVelocity;
        else player.Animator.speed = 1f;
    }
}