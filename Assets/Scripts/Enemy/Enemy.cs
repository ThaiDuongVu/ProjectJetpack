using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public BoxCollider BoxCollider { get; private set; }

    public Transform rig;
    private Rigidbody[] rigRigidbodies;
    private BoxCollider[] rigColliders;
    private const float RagdollForce = 3f;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        BoxCollider = GetComponent<BoxCollider>();

        rigRigidbodies = rig.GetComponentsInChildren<Rigidbody>();
        rigColliders = rig.GetComponentsInChildren<BoxCollider>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {

    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {

    }

    /// <summary>
    /// Enable enemy rig's ragdoll physics.
    /// </summary>
    private void EnableRagdoll()
    {
        Rigidbody.isKinematic = true;
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
