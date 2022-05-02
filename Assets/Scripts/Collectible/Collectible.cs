using UnityEngine;

public class Collectible : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private DelayedDestroyer _delayedDestroyer;

    protected bool IsCollected { get; set; }
    protected bool CanBeCollected { get; set; }

    private const float CollectDelay = 0.2f;
    private const float CollectInterpolationRatio = 0.1f;
    private const float TimeoutDuration = 10f;

    private static readonly int CollectAnimationTrigger = Animator.StringToHash("collect");
    private Transform _collectTarget;

    #region Unity Event

    public virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _delayedDestroyer = GetComponent<DelayedDestroyer>();
    }

    public virtual void Start()
    {
        Invoke(nameof(EnableCollect), CollectDelay);
        Invoke(nameof(Timeout), TimeoutDuration);
    }

    public virtual void FixedUpdate()
    {
        if (!CanBeCollected || !IsCollected || !_collectTarget) return;
        transform.position = Vector2.Lerp(transform.position, _collectTarget.position, CollectInterpolationRatio);
    }

    #endregion

    protected void EnableCollect()
    {
        CanBeCollected = true;
    }

    private void Timeout()
    {
        _animator.SetTrigger(CollectAnimationTrigger);
        StartCoroutine(_delayedDestroyer.Destroy());
    }

    public virtual void OnCollected(Transform target)
    {
        _collectTarget = target;
        IsCollected = true;

        _animator.SetTrigger(CollectAnimationTrigger);
        _rigidbody2D.gravityScale = 0f;
        StartCoroutine(_delayedDestroyer.Destroy());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) OnCollected(other.transform);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player")) OnCollected(other.transform);
    }
}
