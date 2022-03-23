using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Character _character;

    public bool IsRunning { get; set; }
    public bool IsAccelerating { get; set; }

    public Vector2 CurrentDirection { get; set; }
    public float CurrentVelocity { get; set; }

    [Header("Movement Properties")]
    public float maxVelocity;
    public float minVelocity;
    public float acceleration;
    public float deceleration;
    private static readonly int IsRunningAnimationTrigger = Animator.StringToHash("isRunning");


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
        if (GameController.Instance.State == GameState.Paused)
        {
            if (IsRunning) StopRunningImmediate();
            return;
        }

        if (IsAccelerating) Accelerate();
        else Decelerate();

        if (IsRunning) Run();

        ScaleAnimation();
    }

    #endregion

    #region Run Methods

    public virtual void StartRunning(Vector2 direction)
    {
        CurrentDirection = direction;

        IsAccelerating = true;
        IsRunning = true;

        if (_character.Animator) _character.Animator.SetBool(IsRunningAnimationTrigger, true);
    }

    public virtual void StopRunning()
    {
        IsAccelerating = false;
    }

    public virtual void StopRunningImmediate()
    {
        IsAccelerating = false;
        CurrentVelocity = minVelocity;
        IsRunning = false;

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

    public virtual void Run()
    {
        // _character.Rigidbody2D.MovePosition(_character.Rigidbody2D.position +
        //                                     CurrentDirection * CurrentVelocity * Time.fixedDeltaTime);
        _character.Rigidbody2D.velocity = new Vector2(CurrentDirection.x * CurrentVelocity,
                                                      _character.Rigidbody2D.velocity.y);
        _character.SetFlipped(CurrentDirection.x < 0f);
    }

    #endregion

    private void ScaleAnimation()
    {
        if (!_character.Animator) return;
        _character.Animator.speed = IsRunning && CurrentVelocity >= 0 ? CurrentVelocity / maxVelocity : 1f;
    }
}