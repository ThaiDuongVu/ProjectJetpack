using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public SphereCollider SphereCollider { get; private set; }
    public BoxCollider BoxCollider { get; private set; }
    public DestroyDelay DestroyDelay { get; private set; }
    public CollectableSpawner CollectableSpawner { get; private set; }

    public bool IsStagger { get; set; }
    public bool IsDead { get; set; }

    public Transform rig;
    private Rigidbody[] rigRigidbodies;
    private BoxCollider[] rigColliders;
    private const float RagdollForce = 5f;

    [SerializeField] private float maxHealth;
    private float currentHealth;

    private static readonly int DestroyAnimationTrigger = Animator.StringToHash("destroy");

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        SphereCollider = GetComponent<SphereCollider>();
        BoxCollider = GetComponent<BoxCollider>();
        DestroyDelay = GetComponent<DestroyDelay>();
        CollectableSpawner = GetComponent<CollectableSpawner>();

        DestroyDelay.enabled = false;
        rigRigidbodies = rig.GetComponentsInChildren<Rigidbody>();
        rigColliders = rig.GetComponentsInChildren<BoxCollider>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {

    }

    /// <summary>
    /// Deal damage to current enemy.
    /// </summary>
    /// <param name="damage">Damage to deal</param>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f) Die();
    }

    /// <summary>
    /// Handle current enemy death.
    /// </summary>
    public void Die()
    {
        CollectableSpawner.Spawn();
        GameController.Instance.StartCoroutine(GameController.Instance.SlowDownEffect());
        EnableRagdoll();
        IsDead = true;
        DestroyDelay.enabled = true;
    }

    /// <summary>
    /// Enable enemy rig's ragdoll physics.
    /// </summary>
    private void EnableRagdoll()
    {
        Rigidbody.isKinematic = true;
        SphereCollider.enabled = false;
        BoxCollider.enabled = false;
        Animator.enabled = false;

        foreach (var body in rigRigidbodies)
        {
            body.isKinematic = false;
            body.AddForce(new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)) * RagdollForce, ForceMode.Impulse);
        }
        foreach (var collider in rigColliders) collider.enabled = true;
    }
}
