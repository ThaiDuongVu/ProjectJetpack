using System.Collections.Generic;
using System.Collections;
using UnityEngine;

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
    public CircleCollider2D CircleCollider2D;
    public BoxCollider2D BoxCollider2D;

    #endregion

    #region Player States

    public PlayerState State { get; set; } = PlayerState.Idle;
    private const float StaggerDuration = 0.5f;

    #endregion

    #region Player Upgrades

    public List<PlayerUpgrade> upgrades = new List<PlayerUpgrade>();

    #endregion

    #region Prefab References

    public LineRenderer dashSlicePrefab;
    public ParticleSystem bloodSpatPrefab;

    #endregion

    [SerializeField] private Trail trailPrefab;
    public Trail Trail { get; set; }

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
    }

    /// <summary>
    /// Deal damage to player.
    /// </summary>
    /// <param name="damage">Damage to deal</param>
    void IDamageable.TakeDamage(float damage, Vector2 direction)
    {
        Resources.CurrentHealth -= damage;

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
        if (State == PlayerState.Stagger) yield return null;

        State = PlayerState.Stagger;
        Animator.SetTrigger("enterStagger");

        yield return new WaitForSeconds(StaggerDuration);

        State = PlayerState.Idle;
        Animator.SetTrigger("exitStagger");
        Animator.ResetTrigger("enterStagger");
    }

    /// <summary>
    /// Handle player collision with another object.
    /// </summary>
    /// <param name="other">Collision object</param>
    private void OnCollisionEnter2D(Collision2D other)
    {

    }

    /// <summary>
    /// Handle player trigger collision with another object.
    /// </summary>
    /// <param name="other">Collider object</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            if (State != PlayerState.Flying) return;
            
            // Get enemy to damage
            Enemy enemy = other.GetComponent<Enemy>();
            // If enemy cannot be damaged then do nothing
            if (!enemy.CanTakeDamage) return;

            // Deal damage to enemy
            Combat.DealDamage(enemy);
            enemy.CanTakeDamage = false;
        }
    }

    /// <summary>
    /// Handle player trigger collision exit with another object.
    /// </summary>
    /// <param name="other">Collider object</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.CanTakeDamage = true;
        }
    }
}