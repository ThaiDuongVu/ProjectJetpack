using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Character Character { get; private set; }

    public bool IsRunning { get; set; }
    public bool IsAccelerating { get; set; }
    public bool IsFalling { get; set; }
    public bool IsGrounded { get; set; }

    [SerializeField] private float groundRaycastDistance = 0.55f;

    #region Run Properties

    [Header("Movement direction & velocity")]
    private Vector2 currentDirection;
    private float currentVelocity;
    [SerializeField] protected float maxVelocity;
    [SerializeField] protected float minVelocity;
    [SerializeField] protected float acceleration;
    [SerializeField] protected float deceleration;

    #endregion

    #region Jump Properties

    [Header("Jump")]
    [SerializeField] protected float jumpForce;

    #endregion

    #region Animation Properties

    private static readonly int IsRunningAnimationTrigger = Animator.StringToHash("isRunning");
    private static readonly int IsFallingAnimationTrigger = Animator.StringToHash("isFalling");

    #endregion

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    public virtual void Awake()
    {
        Character = GetComponent<Character>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    public virtual void Start()
    {

    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    public virtual void FixedUpdate()
    {
        if (IsAccelerating) Accelerate();
        else Decelerate();

        if (IsRunning) Run();

        DetectFall();
        DetectGround();

        ScaleAnimation();
    }

    #region Run Methods

    /// <summary>
    /// Character start running at a direction.
    /// </summary>
    /// <param name="direction">Direction to start running at</param>
    public virtual void StartRunning(Vector2 direction)
    {
        currentDirection = direction;

        IsAccelerating = true;
        IsRunning = true;

        Character.SetFlipped(direction.x < 0f);
        Character.Animator.SetBool(IsRunningAnimationTrigger, true);
    }

    /// <summary>
    /// Character stop running.
    /// </summary>
    public virtual void StopRunning()
    {
        IsAccelerating = false;
    }

    /// <summary>
    /// Character stop running immediately without deceleration.
    /// </summary>
    public virtual void StopRunningImmediate()
    {
        IsAccelerating = false;
        currentVelocity = minVelocity;
        IsRunning = false;

        Character.Animator.SetBool(IsRunningAnimationTrigger, false);
    }

    /// <summary>
    /// Accelerate character until max velocity reached.
    /// </summary>
    private void Accelerate()
    {
        if (currentVelocity < maxVelocity) currentVelocity += acceleration * Time.fixedDeltaTime;
        else currentVelocity = maxVelocity;
    }

    /// <summary>
    /// Decelerate character until min velocity reached.
    /// </summary>
    private void Decelerate()
    {
        if (currentVelocity > minVelocity) currentVelocity -= deceleration * Time.fixedDeltaTime;
        else
        {
            currentVelocity = minVelocity;
            IsRunning = false;

            Character.Animator.SetBool(IsRunningAnimationTrigger, false);
        }
    }

    /// <summary>
    /// Character run at current direction and velocity.
    /// </summary>
    private void Run()
    {
        Character.Rigidbody2D.velocity = new Vector2(currentDirection.x * currentVelocity, Character.Rigidbody2D.velocity.y);
    }

    #endregion

    /// <summary>
    /// Scale animation speed depends on current velocity.
    /// </summary>
    private void ScaleAnimation()
    {
        Character.Animator.speed = IsRunning && currentVelocity >= 0 ? currentVelocity / maxVelocity : 1f;
    }

    #region Detection Methods

    /// <summary>
    /// Detect whether current player is falling based on current velocity.
    /// </summary>
    private void DetectFall()
    {
        if (Character.Rigidbody2D.velocity.y < 0f)
        {
            if (IsFalling) return;

            IsFalling = true;
            Character.Animator.SetBool(IsFallingAnimationTrigger, true);
        }
        else
        {
            if (!IsFalling) return;

            IsFalling = false;
            Character.Animator.SetBool(IsFallingAnimationTrigger, false);
        }
    }

    /// <summary>
    /// Perform raycast to the ground to check whether player is grounded.
    /// </summary>
    private void DetectGround()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance))
        {
            if (!IsGrounded) IsGrounded = true;
        }
        else
        {
            if (IsGrounded) IsGrounded = false;
        }
    }

    #endregion

    /// <summary>
    /// Apply force upward to jump character.
    /// </summary>
    public virtual void Jump()
    {
        // Reset vertical velocity first
        Character.Rigidbody2D.velocity = new Vector2(Character.Rigidbody2D.velocity.x, 0f);
        // Apply force to jump
        Character.Rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
