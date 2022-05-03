using UnityEngine;

public class ExplodingPlatform : Platform
{
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private int damage = 2;
    [SerializeField] private float explodingForce = 15f;

    private Timer explodingTimer;
	private const float MaxTimer = 2f;
    private bool _timerStarted;

    private Animator _animator;
    private static readonly int FlashAnimationTrigger = Animator.StringToHash("flash");

    private Player _player;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
    }

    public override void Start()
    {
        base.Start();

        explodingTimer = new Timer(MaxTimer);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_timerStarted && explodingTimer.IsReached()) Explode();
    }

    #endregion

    public void Explode()
    {
        if (_player)
        {
            _player.TakeDamage(damage);
            _player.KnockBack((_player.transform.position - transform.position).normalized, explodingForce);
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());
        CameraShaker.Instance.Shake(CameraShakeMode.Light);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Player")) return;

        _animator.SetTrigger(FlashAnimationTrigger);
        _timerStarted = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.transform.CompareTag("Player")) return;

        _player = other.transform.GetComponent<Player>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.transform.CompareTag("Player")) return;

        _player = null;
    }
}
