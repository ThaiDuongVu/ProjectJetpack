using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Animator Animator { get; set; }
    private static readonly int CollectAnimationTrigger = Animator.StringToHash("collect");

    public Rigidbody Rigidbody { get; set; }
    private const float InitForce = 1f;

    public DestroyDelay DestroyDelay { get; set; }

    protected Player target;
    private bool isCollected;

    private const float CollectInterpolatioRatio = 0.2f;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        DestroyDelay = GetComponent<DestroyDelay>();

        DestroyDelay.enabled = false;
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        Rigidbody.AddForce(new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)) * InitForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        if (isCollected) transform.position = Vector3.Lerp(transform.position, target.transform.position, CollectInterpolatioRatio);
    }

    protected virtual void OnCollected() { }

    /// <summary>
    /// Handle trigger collision with other colliders.
    /// </summary>
    /// <param name="other">Collider to check</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        target = other.GetComponent<Player>();
        isCollected = true;
        Animator.SetTrigger(CollectAnimationTrigger);
        OnCollected();

        DestroyDelay.enabled = true;
    }
}
