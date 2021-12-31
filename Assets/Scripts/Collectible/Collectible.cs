using UnityEngine;

public class Collectible : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public CircleCollider2D[] CircleCollider2Ds { get; private set; }
    public DelayedDestroyer DelayedDestroyer { get; private set; }

    private const float TimeoutDuration = 10f;

    protected bool isCollected;
    private bool canBeCollected;
    private const float CollectDelay = 1f;
    private const float CollectInterpolationRatio = 0.15f;

    private static readonly int CollectAnimationTrigger = Animator.StringToHash("collect");
    private Transform collectTarget;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    public virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        CircleCollider2Ds = GetComponents<CircleCollider2D>();
        DelayedDestroyer = GetComponent<DelayedDestroyer>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    public virtual void Start()
    {
        Invoke(nameof(EnableCollect), CollectDelay);
        Invoke(nameof(Timeout), TimeoutDuration);
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    public virtual void FixedUpdate()
    {
        if (!canBeCollected || !isCollected) return;
        
        transform.position = Vector3.Lerp(transform.position, collectTarget.position, CollectInterpolationRatio);

        Rigidbody2D.isKinematic = true;
        foreach (var circleCollider2D in CircleCollider2Ds) circleCollider2D.enabled = false;
        Animator.SetTrigger(CollectAnimationTrigger);
    }

    /// <summary>
    /// Destroy collectible if not collected after a period of time.
    /// </summary>
    private void Timeout()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Enable this object to be collectible.
    /// </summary>
    private void EnableCollect()
    {
        canBeCollected = true;
    }

    /// <summary>
    /// On object collected by another object.
    /// </summary>
    /// <param name="target">Collect target</param>
    public virtual void OnCollected(Transform target)
    {
        collectTarget = target;

        isCollected = true;
        StartCoroutine(DelayedDestroyer.Destroy());
    }

    /// <summary>
    /// Unity Event function.
    /// Handle trigger collision enter with another collider.
    /// </summary>
    /// <param name="other">Other collider to handle</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) OnCollected(other.transform);
    }

    /// <summary>
    /// Unity Event function.
    /// Handle trigger collision stay with another collider.
    /// </summary>
    /// <param name="other">Other collider to handle</param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player")) OnCollected(other.transform);
    }
}
