using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Character _character;

    private bool _isRunning;
    private bool _isAccelerating;

    protected Vector2 CurrentDirection { get; private set; }
    private float CurrentVelocity { get; set; }

    [Header("Movement Properties")]
    public float maxVelocity;
    public float minVelocity;
    public float acceleration;
    public float deceleration;
    public static readonly int IsRunningAnimationTrigger = Animator.StringToHash("isRunning");

    #region Unity Event

    public virtual void Awake()
    {
        _character = GetComponent<Character>();
    }

    public virtual void Start()
    {
    }

    public virtual void FixedUpdate()
    {
        if (GameController.Instance && GameController.Instance.State == GameState.Paused)
        {
            if (_isRunning) StopRunningImmediate();
            return;
        }

        if (_isAccelerating) Accelerate();
        else Decelerate();

        if (_isRunning) Run();

        ScaleAnimation();
    }

    #endregion

    #region Run Methods

    public virtual void StartRunning(Vector2 direction)
    {
        CurrentDirection = direction;

        _isAccelerating = true;
        _isRunning = true;

        if (_character.Animator) _character.Animator.SetBool(IsRunningAnimationTrigger, true);
    }

    protected void StopRunning()
    {
        _isAccelerating = false;
    }

    public void StopRunningImmediate()
    {
        _isAccelerating = false;
        CurrentVelocity = minVelocity;
        _isRunning = false;

        if (_character.Animator) _character.Animator.SetBool(IsRunningAnimationTrigger, false);
    }

    private void Accelerate()
    {
        if (CurrentVelocity < maxVelocity) CurrentVelocity += acceleration * Time.fixedDeltaTime;
        else CurrentVelocity = maxVelocity;
    }

    private void Decelerate()
    {
        if (CurrentVelocity > minVelocity) CurrentVelocity -= deceleration * Time.fixedDeltaTime;
        else StopRunningImmediate();
    }

    private void Run()
    {
        _character.Rigidbody2D.velocity = new Vector2(CurrentDirection.x * CurrentVelocity, _character.Rigidbody2D.velocity.y);
        _character.SetFlipped(CurrentDirection.x < 0f);
    }

    #endregion

    private void ScaleAnimation()
    {
        if (!_character.Animator) return;
        _character.Animator.speed = _isRunning && CurrentVelocity >= 0 ? CurrentVelocity / maxVelocity : 1f;
    }
}
