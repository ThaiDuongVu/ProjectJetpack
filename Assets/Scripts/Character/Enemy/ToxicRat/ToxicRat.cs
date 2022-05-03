using System.Collections;
using UnityEngine;

public class ToxicRat : Enemy
{
    public ToxicRatState State { get; set; } = ToxicRatState.Idle;
    public ToxicRatMovement ToxicRatMovement { get; set; }
    public ToxicRatResources ToxicRatResources { get; set; }

    [SerializeField] private Vector2 wanderDurationRange = new Vector2(2f, 4f);
    [SerializeField] private Vector2 idleDurationRange = new Vector2(1f, 2f);
    public Vector2 Direction { get; set; }

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        ToxicRatMovement = GetComponent<ToxicRatMovement>();
        ToxicRatResources = GetComponent<ToxicRatResources>();
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

        if (IsEdged && State == ToxicRatState.Wander)
        {
            CancelInvoke();
            StopWandering();
        }
    }

    #endregion

    private void StartWandering()
    {
        Direction = -Direction;
        SetFlipped(!IsFlipped);
        ToxicRatMovement.StartRunning(Direction);
        State = ToxicRatState.Wander;

        Invoke(nameof(StopWandering), Random.Range(wanderDurationRange.x, wanderDurationRange.y));
    }

    private void StopWandering()
    {
        ToxicRatMovement.StopRunningImmediate();
        State = ToxicRatState.Idle;

        Invoke(nameof(StartWandering), Random.Range(idleDurationRange.x, idleDurationRange.y));
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
		base.OnCollisionEnter2D(other);

        CancelInvoke();
        StopWandering();
    }
}
