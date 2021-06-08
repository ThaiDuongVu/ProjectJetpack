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

    private float LookInterpolationRatio { get; set; } = 0.25f;

    public float DashDistance { get; set; } = 15f;
    public float DashEpsilon { get; set; } = 1f;
    public float DashInterpolationRatio { get; set; } = 0.22f;
    public Vector2 DashPosition { get; private set; }
    public float DashRate { get; set; } = 2.5f;
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

    /// <summary>
    /// On dash input started.
    /// </summary>
    /// <param name="context">Input context</param>
    private void DashOnStarted(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance.State != GameState.Started || player.IsStaggered) return;

        if (DashCooldown >= 1f / DashRate)
            EnterDash();
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

        // Perform dash if player is dashing
        if (player.IsDashing) Dash();
        if (DashCooldown < 1f / DashRate) DashCooldown += Time.fixedDeltaTime;
        player.directionArrow.localScale = new Vector3(DashCooldown * DashRate, DashCooldown * DashRate, 1f);

        // Move player at current velocity
        if (!player.IsStaggered) Run();
        // Sync player animation with current velocity
        if (!player.IsStaggered) Animate();

        if (player.upgrades[0].isActive) UpdatePreviewRig();
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
        if (player.IsDashing) return;

        // Movement vector derived from input direction and camera angle
        movement = Quaternion.Euler(0f, 0f, camera.transform.eulerAngles.z) * currentDirection;
        // Move player
        player.Rigidbody2D.MovePosition(
            player.Rigidbody2D.position + movement * (currentVelocity * Time.fixedDeltaTime));
        // Rotate player to movement direction
        player.transform.up = Vector2.Lerp(player.transform.up, movement, LookInterpolationRatio);
        // player.transform.up = Vector2.Lerp(player.transform.up, camera.transform.up, LookInterpolationRatio);
    }

    /// <summary>
    /// Enter dash state.
    /// </summary>
    private void EnterDash()
    {
        // Update player state
        player.IsDashing = true;
        player.CircleCollider2D.enabled = false;
        // Set dash position
        if (player.WallPoint == Vector2.zero) DashPosition = player.transform.position + player.transform.up * (DashDistance + DashEpsilon);
        else DashPosition = player.WallPoint - (Vector2)player.transform.up * (DashEpsilon);
        // Set player trail color & animation
        player.Trail.SetColor(player.blue);
        player.Animator.SetTrigger(EnterDashAnimationTrigger);

        DashCooldown = 0f;
        // Enable motion blur effect
        // EffectsController.Instance.SetMotionBlur(true);

        // Deal damage to enemy if acquired
        if (player.Target) player.Combat.DealDamage(player.Target);
        else CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    /// <summary>
    /// Exit dash state.
    /// </summary>
    public void ExitDash()
    {
        // Update player state
        player.IsDashing = false;
        player.CircleCollider2D.enabled = true;
        // Reset player trail color & animation
        player.Trail.SetColor(player.white);
        player.Animator.SetTrigger(ExitDashAnimationTrigger);

        // Disable motion blur effect
        // EffectsController.Instance.SetMotionBlur(false);
    }

    /// <summary>
    /// Perform a dash move.
    /// </summary>
    private void Dash()
    {
        player.transform.position = Vector2.Lerp(player.transform.position, DashPosition, DashInterpolationRatio);
        if (((Vector2)player.transform.position - DashPosition).magnitude <= DashEpsilon)
            ExitDash();
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

    /// <summary>
    /// Update player preview rig to reflect dash position.
    /// </summary>
    private void UpdatePreviewRig()
    {
        // Enable player preview if upgrade enable
        player.previewRig.SetActive(true);
        // Set rig position & scale
        player.previewRig.transform.position = player.transform.position + player.transform.up * DashDistance * DashCooldown * DashRate;
        player.previewRig.transform.localScale = new Vector3(DashCooldown * DashRate, DashCooldown * DashRate, 1f);
    }
}