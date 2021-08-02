using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible

    #region Singleton

    private static Player instance;

    public static Player Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<Player>();
            return instance;
        }
    }

    #endregion

    #region Player Components

    public PlayerMovement Movement { get; private set; }
    public PlayerCombat Combat { get; private set; }
    public PlayerCombo Combo { get; private set; }
    public PlayerResources Resources { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public BoxCollider BoxCollider { get; private set; }
    public Trail Trail { get; set; }
    public Enemy Target { get; set; }

    #endregion

    #region Player States

    public bool IsRunning { get; set; }
    public bool IsDashing { get; set; }
    public bool IsStaggered { get; set; }
    public bool IsDead { get; set; }
    private const float StaggerDuration = 0.5f;

    #endregion

    #region Prefab References

    public LineRenderer slicePrefab;
    public ParticleSystem bloodSpatPrefab;
    public Trail trailPrefab;

    #endregion

    public Color white;
    public Color blue;
    public Color red;
    public Transform directionArrow;
    public Material arrowMaterial;

    public Transform rig;
    private Rigidbody[] rigRigidbodies;
    private BoxCollider[] rigColliders;
    private const float RagdollForce = 5f;

    private static readonly int EnterStaggerAnimationTrigger = Animator.StringToHash("enterStagger");
    private static readonly int ExitStaggerAnimationTrigger = Animator.StringToHash("exitStagger");

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        Movement = GetComponent<PlayerMovement>();
        Combat = GetComponent<PlayerCombat>();
        Combo = GetComponent<PlayerCombo>();
        Resources = GetComponent<PlayerResources>();

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
        Trail = Instantiate(trailPrefab, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.identity).GetComponent<Trail>();
        Trail.Target = transform;
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        CheckTarget();
    }

    /// <summary>
    /// Perform a raycast from player forward to check if any target is within dash range.
    /// </summary>
    private void CheckTarget()
    {
        Target = null;
        arrowMaterial.color = white;

        if (Movement.DashCooldown < 1f / Movement.DashRate) return;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, Movement.DashDistance - Movement.DashEpsilon, LayerMask.GetMask("Enemy"));
        if (hits.Length <= 0) return;

        if (hits[0].transform.CompareTag("Enemy"))
        {
            Target = hits[0].transform.GetComponent<Enemy>();
            arrowMaterial.color = red;
        }
    }

    /// <summary>
    /// Add stagger effects to player.
    /// </summary>
    private IEnumerator Stagger()
    {
        if (IsStaggered) yield return null;

        IsStaggered = true;
        Animator.SetTrigger(EnterStaggerAnimationTrigger);

        yield return new WaitForSeconds(StaggerDuration);

        IsStaggered = false;
        Animator.SetTrigger(ExitStaggerAnimationTrigger);
        Animator.ResetTrigger(EnterStaggerAnimationTrigger);
    }

    /// <summary>
    /// Enable player rig's ragdoll physics.
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
