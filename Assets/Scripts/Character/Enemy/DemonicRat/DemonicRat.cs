using UnityEngine;

public class DemonicRat : Enemy
{
    public DemonicRatMovement DemonicRatMovement { get; private set; }
    public DemonicRatCombat DemonicRatCombat { get; private set; }
    public DemonicRatResources DemonicRatResources { get; private set; }

    public DemonicRatState State { get; set; } = DemonicRatState.Idle;
    private Vector2 _idleTimeRange = new Vector2(1f, 2f);
    public Vector2 Direction { get; set; }

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        DemonicRatMovement = GetComponent<DemonicRatMovement>();
        DemonicRatCombat = GetComponent<DemonicRatCombat>();
        DemonicRatResources = GetComponent<DemonicRatResources>();
    }

    public override void Start()
    {
        base.Start();

        var initDirections = new[] { -1f, 1f };
        Direction = new Vector2(initDirections[Random.Range(0, initDirections.Length)], 0f);
        WanderCurrentDirection();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    #endregion

    private void WanderOppositeDirection()
    {
        Direction = -Direction;
        State = DemonicRatState.Wander;
        DemonicRatMovement.StartRunning(Direction);
    }

    private void WanderCurrentDirection()
    {
        State = DemonicRatState.Wander;
        DemonicRatMovement.StartRunning(Direction);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DemonicRatBorder") && (other.transform.position.x - transform.position.x) * Direction.x >= 0f)
        {
            DemonicRatMovement.StopRunning();
            State = DemonicRatState.Idle;
            Invoke(nameof(WanderOppositeDirection), Random.Range(_idleTimeRange.x, _idleTimeRange.y));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            DemonicRatMovement.StopRunning();
            State = DemonicRatState.Idle;
            Invoke(nameof(WanderCurrentDirection), Random.Range(_idleTimeRange.x, _idleTimeRange.y));
        }
    }
}
