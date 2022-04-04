using UnityEngine;

public class DemonicRat : Enemy
{
    public DemonicRatState State { get; set; } = DemonicRatState.Idle;

    public DemonicRatResources DemonicRatResources { get; private set; }
    public DemonicRatMovement DemonicRatMovement { get; private set; }
    public DemonicRatPathfinder DemonicPathfinder { get; private set; }

    private Transform _rushTarget;
    [SerializeField] private float pathfindingUpdateRate = 1f;

    [SerializeField] private ParticleSystem bloodSplashRedPrefab;

    private static readonly int AttackAnimationTrigger = Animator.StringToHash("attack");

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        DemonicRatResources = GetComponent<DemonicRatResources>();
        DemonicRatMovement = GetComponent<DemonicRatMovement>();
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
            StartWandering();
        }
    }

    #endregion

    #region Rush Methods

    private void StartRushing()
    {
        if (!_rushTarget) return;

        DemonicPathfinder.FindPath(_rushTarget);
        DemonicPathfinder.StartTracking();
        State = DemonicRatState.Rush;
    }

    private void StopRushing()
    {
        DemonicPathfinder.StopTracking();
        State = DemonicRatState.Idle;
    }

    #endregion

    public void StartWandering()
    {
        DemonicPathfinder.FindPath(new Vector2(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y)));
        DemonicPathfinder.StartTracking();
        State = DemonicRatState.Wander;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Player")) return;

        Animator.SetTrigger(AttackAnimationTrigger);

        var player = other.transform.GetComponent<Player>();
        player.TakeDamage(DemonicRatResources.damage);
        player.Stagger(DemonicRatMovement.CurrentDirection);

        Instantiate(bloodSplashRedPrefab, player.transform.position, Quaternion.identity).transform.up = DemonicRatMovement.CurrentDirection;
        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
        StopRushing();
    }
}