using UnityEngine;

public class BeamTurtle : Enemy
{
    private BeamTurtleState State { get; set; } = BeamTurtleState.Idle;
    private BeamTurtleMovement _beamTurtleMovement;

    [SerializeField] private Vector2 wanderDurationRange = new Vector2(2f, 4f);
    [SerializeField] private Vector2 idleDurationRange = new Vector2(2f, 4f);
    private Vector2 _direction;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _beamTurtleMovement = GetComponent<BeamTurtleMovement>();
        GetComponent<BeamTurtleResources>();
    }

    public override void Start()
    {
        base.Start();

        _direction = new Vector2(Random.Range(-1f, 1f), 0f).normalized;
        Invoke(nameof(StartWandering), Random.Range(idleDurationRange.x, idleDurationRange.y));
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        DetectEdge();

        if (!IsEdged || State != BeamTurtleState.Wander) return;
        
        CancelInvoke();
        StopWandering();
    }

    #endregion

    private void StartWandering()
    {
        _direction = -_direction;
        SetFlipped(!IsFlipped);
        _beamTurtleMovement.StartRunning(_direction);
        State = BeamTurtleState.Wander;

        Invoke(nameof(StopWandering), Random.Range(wanderDurationRange.x, wanderDurationRange.y));
    }

    private void StopWandering()
    {
        _beamTurtleMovement.StopRunningImmediate();
        State = BeamTurtleState.Idle;

        Invoke(nameof(StartWandering), Random.Range(idleDurationRange.x, idleDurationRange.y));
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        
        CancelInvoke();
        StopWandering();
    }
}
