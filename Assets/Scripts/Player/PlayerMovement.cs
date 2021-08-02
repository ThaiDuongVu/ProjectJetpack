using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private new Camera camera;

    private Vector3 movement;
    private Vector3 currentDirection;
    private float currentVelocity;

    public float MaxVelocity { get; set; } = 20f;
    public float MinVelocity { get; set; } = 0f;
    public float Acceleration { get; set; } = 60f;
    public float Deceleration { get; set; } = 60f;

    private static readonly int IsRunningAnimationTrigger = Animator.StringToHash("isRunning");
    private static readonly int EnterDashAnimationTrigger = Animator.StringToHash("enterDash");
    private static readonly int ExitDashAnimationTrigger = Animator.StringToHash("exitDash");

    private float LookInterpolationRatio { get; set; } = 0.2f;

    public float DashDistance { get; set; } = 15f;
    public float DashEpsilon { get; set; } = 1f;
    public float DashInterpolationRatio { get; set; } = 0.2f;
    public Vector3 DashPosition { get; private set; }
    public float DashRate { get; set; } = 2f;
    public float DashCooldown { get; private set; }

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

        // Handle dash input
        inputManager.Player.Dash.started += DashOnStarted;

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

        StartRunning(new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y));
        player.IsRunning = true;
    }

    /// <summary>
    /// On move input canceled.
    /// </summary>
    /// <param name="context">Input context</param>
    private void MoveOnCanceled(InputAction.CallbackContext context)
    {
        player.IsRunning = false;
    }

    /// <summary>
    /// On dash input started.
    /// </summary>
    /// <param name="context">Input context</param>
    private void DashOnStarted(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance.State != GameState.Started || player.IsStaggered) return;

        if (DashCooldown >= 1f / DashRate) EnterDash();
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
        player = GetComponent<Player>();
        camera = Camera.main;
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;

        if (player.IsRunning) Accelerate();
        else Decelerate();

        if (player.IsDashing) Dash();
        if (DashCooldown < 1f / DashRate) DashCooldown += Time.fixedDeltaTime;

        player.directionArrow.localScale = new Vector3(DashCooldown * DashRate, 1f, DashCooldown * DashRate);

        if (!player.IsStaggered) Run();
        if (!player.IsStaggered) Animate();
    }

    /// <summary>
    /// Player enter running state.
    /// </summary>
    private void StartRunning(Vector3 direction)
    {
        currentDirection = direction;
        player.Animator.SetBool(IsRunningAnimationTrigger, true);
    }

    /// <summary>
    /// Player exit running state.
    /// </summary>
    private void StopRunning()
    {
        currentDirection = Vector3.zero;
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
        // If player near stopping then fully stop
        else StopRunning();
    }

    /// <summary>
    /// Move player to movement vector.
    /// </summary>
    private void Run()
    {
        if (player.IsDashing) return;

        // Movement vector derived from input direction and camera angle
        movement = Quaternion.Euler(0f, camera.transform.eulerAngles.y, 0f) * currentDirection;
        // Move & rotate player to movement direction
        player.Rigidbody.velocity = movement * currentVelocity;
        player.transform.forward = Vector3.Lerp(player.transform.forward, movement, LookInterpolationRatio);
    }

    /// <summary>
    /// Player enter dash state.
    /// </summary>
    private void EnterDash()
    {
        player.IsDashing = true;
        player.BoxCollider.enabled = false;

        DashPosition = player.transform.position + player.transform.forward * (DashDistance + DashEpsilon);

        player.Trail.SetColor(player.blue);
        player.Animator.SetTrigger(EnterDashAnimationTrigger);

        DashCooldown = 0f;

        // Deal damage to enemy if applicable
        if (player.Target) player.Combat.DealDamage(player.Target, 1f);
        else CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    /// <summary>
    /// Player exit dash state.
    /// </summary>
    public void ExitDash()
    {
        player.IsDashing = false;
        player.BoxCollider.enabled = true;

        player.Trail.SetColor(player.white);
        player.Animator.SetTrigger(ExitDashAnimationTrigger);
    }

    /// <summary>
    /// Perform a dash move.
    /// </summary>
    private void Dash()
    {
        player.transform.position = Vector3.Lerp(player.transform.position, DashPosition, DashInterpolationRatio);
        if ((player.transform.position - DashPosition).magnitude <= DashEpsilon)
            ExitDash();
    }

    /// <summary>
    /// Scale animation speed to movement speed.
    /// </summary>
    private void Animate()
    {
        if (currentVelocity > 0f) player.Animator.speed = currentVelocity / MaxVelocity;
        else player.Animator.speed = 1f;
    }
}
