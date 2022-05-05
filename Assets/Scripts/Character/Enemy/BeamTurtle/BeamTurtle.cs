using UnityEngine;

public class BeamTurtle : Enemy
{
    public BeamTurtleState State { get; set; } = BeamTurtleState.Idle;
    public BeamTurtleMovement BeamTurtleMovement { get; set; }
    public BeamTurtleResources BeamTurtleResources { get; set; }

    [SerializeField] private Vector2 wanderDurationRange = new Vector2(2f, 4f);
    [SerializeField] private Vector2 idleDurationRange = new Vector2(2f, 4f);
    public Vector2 Direction { get; set; }

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        BeamTurtleMovement = GetComponent<BeamTurtleMovement>();
        BeamTurtleResources = GetComponent<BeamTurtleResources>();
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

        if (IsEdged && State == BeamTurtleState.Wander)
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
        BeamTurtleMovement.StartRunning(Direction);
        State = BeamTurtleState.Wander;

        Invoke(nameof(StopWandering), Random.Range(wanderDurationRange.x, wanderDurationRange.y));
    }

    private void StopWandering()
    {
        BeamTurtleMovement.StopRunningImmediate();
        State = BeamTurtleState.Idle;

        Invoke(nameof(StartWandering), Random.Range(idleDurationRange.x, idleDurationRange.y));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        CancelInvoke();
        StopWandering();
    }
}
