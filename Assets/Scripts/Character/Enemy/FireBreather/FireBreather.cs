using UnityEngine;

public class FireBreather : Enemy
{
    [Header("Fire Properties")]
    [SerializeField] private Transform gunPoint;
    [SerializeField] private Fireball fireballPrefab;
    [SerializeField] private Vector2 fireInterval = new Vector2(2f, 4f);
    [SerializeField] private ParticleSystem fireMuzzlePrefab;
    private Timer _fireTimer;

    private Transform _target;
    [SerializeField] private int maxBulletPopulation = 1;
    public int BulletsCount { get; set; }

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _target = FindObjectOfType<Player>().transform;
    }

    public override void Start()
    {
        base.Start();

        _fireTimer = new Timer(Random.Range(fireInterval.x, fireInterval.y));
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_target) SetFlipped((_target.position - transform.position).normalized.x < 0f);

        if (_fireTimer.IsReached() && BulletsCount < maxBulletPopulation)
        {
            Fire();
            _fireTimer.Reset(Random.Range(fireInterval.x, fireInterval.y));
        }
    }

    #endregion

    private void Fire()
    {
        if (!_target) return;

        var bullet = Instantiate(fireballPrefab, gunPoint.position, Quaternion.identity);
        bullet.FireBreather = this;
        bullet.Target = _target;
        bullet.Sender = transform;

        BulletsCount++;
        Instantiate(fireMuzzlePrefab, gunPoint.position, Quaternion.identity).transform.up = IsFlipped ? Vector2.right : Vector2.left;
    }
}