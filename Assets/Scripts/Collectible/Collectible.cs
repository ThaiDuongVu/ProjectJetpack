using UnityEngine;

public class Collectible : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private Collider2D[] _colliders;
    private DelayedDestroyer _delayedDestroyer;

    protected bool IsCollected { get; private set; }
    protected bool CanBeCollected { get; private set; }

    [SerializeField] private float collectDelay = 0.2f;
    [SerializeField] private float timeoutDuration = 10f;
    private const float CollectInterpolationRatio = 0.1f;

    private static readonly int CollectAnimationTrigger = Animator.StringToHash("collect");
    private Transform _collectTarget;

    #region Unity Event

    public virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _colliders = GetComponents<Collider2D>();
        _delayedDestroyer = GetComponent<DelayedDestroyer>();
    }

    public virtual void Start()
    {
        Invoke(nameof(EnableCollect), collectDelay);
        Invoke(nameof(Timeout), timeoutDuration);
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

    protected virtual void OnCollected(Transform target)
    {
        _collectTarget = target;
        IsCollected = true;

        _animator.SetTrigger(CollectAnimationTrigger);
        _rigidbody2D.gravityScale = 0f;
        foreach (var col in _colliders) col.enabled = false;
        StartCoroutine(_delayedDestroyer.Destroy());
        AudioController.Instance.Play(AudioVariant.PlayerCollectItem);
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
