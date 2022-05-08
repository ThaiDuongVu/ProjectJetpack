using UnityEngine;

public class ShieldedFly : Enemy
{
    private ShieldedFlyMovement _shieldedFlyMovement;

    [SerializeField] private Vector2 wanderDurationRange = new Vector2(2f, 4f);
    [SerializeField] private Vector2 idleDurationRange = new Vector2(1f, 2f);
    private Vector2 _direction;

    [SerializeField] private float knockBackForce = 10f;

    [SerializeField] private FlyShieldSet flyShieldSetPrefab;
    private FlyShieldSet _flyShieldSet;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _shieldedFlyMovement = GetComponent<ShieldedFlyMovement>();
    }

    public override void Start()
    {
        base.Start();

        _direction = new Vector2(Random.Range(-1f, 1f), 0f).normalized;
        Invoke(nameof(StartWandering), Random.Range(idleDurationRange.x, idleDurationRange.y));

        _flyShieldSet = Instantiate(flyShieldSetPrefab, transform.position, Quaternion.identity);
        _flyShieldSet.Target = this;
    }

    #endregion

    public override void Die()
    {
        base.Die();

        _flyShieldSet.Die();
        AudioController.Instance.Play(AudioVariant.Explode2);
        AudioController.Instance.Play(AudioVariant.PlayerReachBasePlatform);
    }

    private void StartWandering()
    {
        _direction = -_direction;
        SetFlipped(!IsFlipped);
        _shieldedFlyMovement.StartRunning(_direction);

        Invoke(nameof(StopWandering), Random.Range(wanderDurationRange.x, wanderDurationRange.y));
    }

    private void StopWandering()
    {
        _shieldedFlyMovement.StopRunningImmediate();

        Invoke(nameof(StartWandering), Random.Range(idleDurationRange.x, idleDurationRange.y));
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Player")) return;
        var player = other.transform.GetComponent<Player>();
        player.KnockBack((player.transform.position - transform.position).normalized, knockBackForce);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CancelInvoke();
        StopWandering();
    }
}
