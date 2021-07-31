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
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public BoxCollider BoxCollider { get; private set; }
    public Trail Trail { get; set; }

    #endregion

    #region Player States

    public bool IsRunning { get; set; }
    public bool IsDashing { get; set; }
    public bool IsStaggered { get; set; }
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
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        BoxCollider = GetComponent<BoxCollider>();
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
}
