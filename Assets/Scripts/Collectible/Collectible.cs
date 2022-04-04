using UnityEngine;

public class Collectible : MonoBehaviour
{
    private Animator _animator;
    private DelayedDestroyer _delayedDestroyer;

    protected bool IsCollected { get; set; }
    protected bool CanBeCollected { get; set; }

    public Vector2 InitPosition { get; set; } = Vector2.zero;
    [SerializeField] private Vector2 minPosition = new Vector2(-14f, -7f);
    [SerializeField] private Vector2 maxPosition = new Vector2(14f, 7f);
    private const float InitInterpolationRatio = 0.05f;

    private const float CollectDelay = 0.5f;
    private const float CollectInterpolationRatio = 0.1f;

    private const float TimeoutDuration = 10f;

    private static readonly int CollectAnimationTrigger = Animator.StringToHash("collect");
    private Transform _collectTarget;

    #region Unity Event

    public virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _delayedDestroyer = GetComponent<DelayedDestroyer>();
    }

    public virtual void Start()
    {
        InitPosition = new Vector2(Mathf.Clamp(InitPosition.x, minPosition.x, maxPosition.x),
            Mathf.Clamp(InitPosition.y, minPosition.y, maxPosition.y));

        Invoke(nameof(EnableCollect), CollectDelay);
        Invoke(nameof(Timeout), TimeoutDuration);
    }

    public virtual void FixedUpdate()
    {
        // Lerp to init position if collectible cannot be collected
        if (!CanBeCollected) transform.position = Vector2.Lerp(transform.position, InitPosition, InitInterpolationRatio);
        // Otherwise lerp to target position
        else
        {
            if (!IsCollected || !_collectTarget) return;
            transform.position = Vector2.Lerp(transform.position, _collectTarget.position, CollectInterpolationRatio);
        }
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

    public virtual void OnCollected(Transform target)
    {
        _collectTarget = target;
        IsCollected = true;

        _animator.SetTrigger(CollectAnimationTrigger);
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