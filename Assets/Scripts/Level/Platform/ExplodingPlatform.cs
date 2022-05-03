using System.Collections.Generic;
using UnityEngine;

public class ExplodingPlatform : Platform
{
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private int damage = 2;
    [SerializeField] private float explodingForce = 15f;

    private Timer explodingTimer;
    private const float MaxTimer = 1f;
    private bool _timerStarted;
    private bool _exploded;

    private Animator _animator;
    private static readonly int FlashAnimationTrigger = Animator.StringToHash("flash");

    private Player _player;
    private List<Enemy> _enemies = new List<Enemy>();

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
        if (_exploded) return;

        if (_player)
        {
            _player.TakeDamage(damage);
            _player.KnockBack((_player.transform.position - transform.position).normalized, explodingForce);
        }
        for (int i = 0; i < _enemies.Count; i++)
        {
            var enemy = _enemies[i];
            enemy.TakeDamage(damage);
            enemy.KnockBack((enemy.transform.position - transform.position).normalized, explodingForce);
        }

        _exploded = true;
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
        if (other.transform.CompareTag("Enemy"))
            _enemies.Add(other.transform.GetComponent<Enemy>());

        if (other.transform.CompareTag("Player"))
            _player = other.transform.GetComponent<Player>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemy") && _enemies.Contains(other.transform.GetComponent<Enemy>()))
            _enemies.Remove(other.transform.GetComponent<Enemy>());

        if (other.transform.CompareTag("Player"))
            _player = null;
    }
}
