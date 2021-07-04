using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public PlayerHud Hud { get; set; }
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public CircleCollider2D CircleCollider2D { get; private set; }
    public Trail Trail { get; set; }
    public Enemy Target { get; set; }

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
    public Color red;
    public Color blue;
    public Transform directionArrow;
    public SpriteRenderer directionArrowSprite;
    public Transform raycastPoint;
    private RaycastHit2D[] hit2Ds;

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

        if (Movement.DashCooldown < 1f / Movement.DashRate) return;

        // Perform raycast and return if nothing detected
        hit2Ds = Physics2D.RaycastAll(raycastPoint.position, transform.up,
                                  Movement.DashDistance + (transform.position.y - raycastPoint.position.y) + Movement.DashEpsilon,
                                  LayerMask.GetMask("Enemy"));

        if (hit2Ds.Length == 0) return;

        // Acquire targets
        if (hit2Ds[0].transform.CompareTag("Enemy"))
        {
            Target = hit2Ds[0].transform.GetComponent<Enemy>();
            directionArrowSprite.color = red;
        }
    }

    /// <summary>
    /// Deal damage to player.
    /// </summary>
    /// <param name="damage">Damage to deal</param>
    public void TakeDamage(float damage, Vector2 direction)
    {
        Resources.CurrentHealth -= damage;
        if (IsDashing) Movement.ExitDash();

        // Flavours
        StartCoroutine(Stagger());
        CameraShaker.Instance.Shake(CameraShakeMode.Normal);

        // If health below 0 then die
        if (Resources.CurrentHealth <= 0f)
            Die();
    }

    /// <summary>
    /// Handle player death.
    /// </summary>    
    public void Die()
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
