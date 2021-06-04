using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageable
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
    public PlayerHud Hud { get; set; }
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public CircleCollider2D CircleCollider2D { get; private set; }

    #endregion

    #region Player States

    public bool IsRunning { get; set; }
    public bool IsDashing { get; set; }
    public bool IsStaggered { get; set; }
    private const float StaggerDuration = 0.5f;

    #endregion

    #region Player Upgrades

    public List<PlayerUpgrade> upgrades = new List<PlayerUpgrade>();
    // Upgrade IDs
    // 0: Preview

    #endregion

    #region Prefab References

    public LineRenderer dashSlicePrefab;
    public ParticleSystem bloodSpatPrefab;

    #endregion

    [SerializeField] private Trail trailPrefab;
    public Trail Trail { get; set; }

    public Color white;
    public Color red;
    public Color blue;
    public Transform directionArrow;
    public SpriteRenderer directionArrowSprite;
    private RaycastHit2D[] hit2Ds;
    public Enemy Target { get; set; }
    public Vector2 WallPoint { get; set; }

    public GameObject previewRig;
    public Transform raycastPoint;

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
        Hud = GetComponent<PlayerHud>();
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        CircleCollider2D = GetComponent<CircleCollider2D>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        Trail = Instantiate(trailPrefab, transform.position, transform.rotation).GetComponent<Trail>();
        Trail.Target = transform;

        previewRig.SetActive(false);
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;

        CheckTarget();
    }

    /// <summary>
    /// Perform a raycast from player forward to check if any target is within dash range.
    /// </summary>
    private void CheckTarget()
    {
        // Default states
        directionArrowSprite.color = white;
        Target = null;
        WallPoint = Vector2.zero;

        if (Movement.DashCooldown < 1f / Movement.DashRate) return;

        // Perform raycast and return if nothing detected
        hit2Ds = Physics2D.RaycastAll(raycastPoint.position, transform.up,
                                  Movement.DashDistance + (transform.position.y - raycastPoint.position.y) + Movement.DashEpsilon,
                                  LayerMask.GetMask("Enemy", "UpgradeActivator", "Wall"));

        if (hit2Ds.Length == 0) return;

        // Acquire targets
        if (hit2Ds[0].transform.CompareTag("Enemy"))
        {
            Target = hit2Ds[0].transform.GetComponent<Enemy>();
            directionArrowSprite.color = red;
        }

        // Confine player within map bound
        foreach (RaycastHit2D hit in hit2Ds)
            if (hit.transform.CompareTag("Wall")) WallPoint = hit.point;
    }

    /// <summary>
    /// Deal damage to player.
    /// </summary>
    /// <param name="damage">Damage to deal</param>
    void IDamageable.TakeDamage(float damage)
    {
        Resources.CurrentHealth -= damage;
        if (IsDashing) Movement.ExitDash();
        // Flavours
        StartCoroutine(Stagger());
        CameraShaker.Instance.Shake(CameraShakeMode.Normal);

        if (Resources.CurrentHealth <= 0f)
            (this as IDamageable).Die();
    }

    /// <summary>
    /// Handle player death.
    /// </summary>
    void IDamageable.Die()
    {
    }

    /// <summary>
    /// Add stagger effects to player.
    /// </summary>
    private IEnumerator Stagger()
    {
        IsStaggered = true;
        Animator.SetTrigger("enterStagger");

        yield return new WaitForSeconds(StaggerDuration);

        IsStaggered = false;
        Animator.SetTrigger("exitStagger");
        Animator.ResetTrigger("enterStagger");
    }

    /// <summary>
    /// Handle player collision with another object.
    /// </summary>
    /// <param>Collision object</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Wall"))
        {

        }
    }
}