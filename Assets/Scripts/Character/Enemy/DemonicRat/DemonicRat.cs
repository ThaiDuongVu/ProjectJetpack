using UnityEngine;

public class DemonicRat : Enemy
{
    public DemonicRatState State { get; set; } = DemonicRatState.Idle;

    public DemonicRatMovement DemonicRatMovement { get; private set; }
    public DemonicRatCombat DemonicRatCombat { get; private set; }
    public DemonicRatResources DemonicRatResources { get; private set; }
    public DemonicRatPathfinder DemonicPathfinder { get; private set; }

    private Transform _rushTarget;
    [SerializeField] private float pathfindingUpdateRate = 1f;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        DemonicRatMovement = GetComponent<DemonicRatMovement>();
        DemonicRatCombat = GetComponent<DemonicRatCombat>();
        DemonicRatResources = GetComponent<DemonicRatResources>();
        DemonicPathfinder = GetComponent<DemonicRatPathfinder>();

        _rushTarget = FindObjectOfType<Player>()?.transform;
    }

    public override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(StartRushing), 0f, pathfindingUpdateRate);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!_rushTarget && State != DemonicRatState.Wander)
        {
            CancelInvoke();
            Wander();
        }
    }

    #endregion

    #region Rush Methods

    private void StartRushing()
    {
        if (!_rushTarget) return;

        DemonicPathfinder.FindPath(_rushTarget);
        DemonicPathfinder.SetTracking(true);
        State = DemonicRatState.Rush;
    }

    public void StopRushing()
    {
        DemonicPathfinder.SetTracking(false);
        State = DemonicRatState.Idle;
    }

    #endregion

    public void Wander()
    {
        DemonicPathfinder.FindPath(new Vector2(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y)));
        DemonicPathfinder.SetTracking(true);
        State = DemonicRatState.Wander;
    }
}