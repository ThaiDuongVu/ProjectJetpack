using UnityEngine;

public class ToxicRat : Enemy
{
    private ToxicRatState _state;
    private ToxicRatMovement _toxicRatMovement;

    [SerializeField] private Vector2 wanderDurationRange = new Vector2(2f, 4f);
    [SerializeField] private Vector2 idleDurationRange = new Vector2(1f, 2f);
    public Vector2 Direction { get; private set; }

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _toxicRatMovement = GetComponent<ToxicRatMovement>();
    }

    public override void Start()
    {
        base.Start();

        Direction = new Vector2(Random.Range(-1f, 1f), 0f).normalized;
        Invoke(nameof(StartWandering), Random.Range(idleDurationRange.x, idleDurationRange.y));
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        DetectEdge();

        if (IsEdged && _state == ToxicRatState.Wander)
        {
            CancelInvoke();
            StopWandering();
        }
    }

    #endregion

    public override void Die()
    {
        base.Die();

        AudioController.Instance.Play(AudioVariant.Explode1);
    }

    private void StartWandering()
    {
        Direction = -Direction;
        SetFlipped(!IsFlipped);
        _toxicRatMovement.StartRunning(Direction);
        _state = ToxicRatState.Wander;

        Invoke(nameof(StopWandering), Random.Range(wanderDurationRange.x, wanderDurationRange.y));
    }

    private void StopWandering()
    {
        _toxicRatMovement.StopRunningImmediate();
        _state = ToxicRatState.Idle;

        Invoke(nameof(StartWandering), Random.Range(idleDurationRange.x, idleDurationRange.y));
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);

        CancelInvoke();
        StopWandering();
    }
}
