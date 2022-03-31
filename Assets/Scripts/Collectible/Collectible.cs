using UnityEngine;

public class Collectible : MonoBehaviour
{
    private Animator _animator;
    private static readonly int CollectAnimationTrigger = Animator.StringToHash("collect");

    public bool IsCollected { get; set; }
    public bool CanBeCollected { get; set; }

    private Transform _collectTarget;
    private const float CollectDelay = 0.5f;
    private const float CollectInterpolationRatio = 0.1f;

    private DelayedDestroyer _delayedDestroyer;
    private const float TimeoutDuration = 30f;

    private Rigidbody2D _rigidbody2D;
    private const float InitForce = 8f;

    #region Unity Event

    public virtual void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _delayedDestroyer = GetComponent<DelayedDestroyer>();
    }

    public virtual void Start()
    {
        _rigidbody2D.AddForce(new Vector2(Random.Range(-1f, 1f), 1f).normalized * InitForce, ForceMode2D.Impulse);
        Invoke(nameof(EnableCollect), CollectDelay);
        Invoke(nameof(Timeout), TimeoutDuration);
    }

    public virtual void FixedUpdate()
    {
        if (!CanBeCollected || !IsCollected || !_collectTarget) return;
        transform.position = Vector2.Lerp(transform.position, _collectTarget.position, CollectInterpolationRatio);
    }

    #endregion

    private void EnableCollect()
    {
        CanBeCollected = true;
    }

    private void Timeout()
    {
        _animator.SetTrigger(CollectAnimationTrigger);
        StartCoroutine(_delayedDestroyer.Destroy());
    }

    public virtual void Collect(Transform target)
    {
        _rigidbody2D.gravityScale = 0f;
        _collectTarget = target;
        IsCollected = true;

        _animator.SetTrigger(CollectAnimationTrigger);
        StartCoroutine(_delayedDestroyer.Destroy());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) Collect(other.transform);
        else if (other.CompareTag("BottomBorder")) Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player")) Collect(other.transform);
    }
}
