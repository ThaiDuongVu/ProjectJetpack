using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public Collider2D Collider2D { get; private set; }

    public CharacterMovement CharacterMovement { get; private set; }
    public CharacterCombat CharacterCombat { get; private set; }
    public CharacterResources CharacterResources { get; private set; }

    public bool IsDead { get; set; }
    [SerializeField] private ParticleSystem explosionPrefab;

    public bool IsFlipped { get; set; }
    private static readonly int StaggerAnimationTrigger = Animator.StringToHash("isStagger");
    public SpriteRenderer mainSprite;

    [Header("Ground Properties")]
    [SerializeField] private float groundRaycastDistance = 0.55f;
    public virtual bool IsGrounded { get; set; }
    public Collider2D GroundPlatform { get; set; }
    private static readonly int FallAnimationTrigger = Animator.StringToHash("isFalling");

    [Header("Edge Properties")]
    [SerializeField] private Transform[] edgePoints;
    [SerializeField] private float edgeRaycastDistance = 0.55f;
    private string[] levelLayers = { "Levels" };
    public virtual bool IsEdged { get; set; }

    #region Unity Event

    public virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();

        CharacterMovement = GetComponent<CharacterMovement>();
        CharacterCombat = GetComponent<CharacterCombat>();
        CharacterResources = GetComponent<CharacterResources>();
    }

    public virtual void Start()
    {
    }

    public virtual void FixedUpdate()
    {

    }

    #endregion

    #region Damage & Death

    public virtual void TakeDamage(int damage)
    {
        CharacterResources.Health -= damage;
    }

    public virtual void Die()
    {
        if (IsDead) return;

        IsDead = true;

        if (explosionPrefab) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    #endregion

    public virtual void SetFlipped(bool value)
    {
        mainSprite.transform.localScale = new Vector2(value ? -1f : 1f, 1f);
        IsFlipped = value;
    }

    public virtual void KnockBack(Vector2 direction, float force)
    {
        if (!Rigidbody2D) return;
        if (CharacterMovement) CharacterMovement.StopRunningImmediate();

        Rigidbody2D.velocity = Vector2.zero;
        Rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public virtual void DetectGrounded()
    {
        IsGrounded = false;

        var hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance);
        if (hit)
        {
            GroundPlatform = hit.transform.GetComponent<Collider2D>();
            IsGrounded = true;
        }

        Animator.SetBool(FallAnimationTrigger, !IsGrounded);
    }

    public virtual void DetectEdge()
    {
        IsEdged = true;

        foreach (var point in edgePoints)
        {
            if (Physics2D.Raycast(point.position, Vector2.down, edgeRaycastDistance, LayerMask.GetMask(levelLayers)))
            {
                IsEdged = false;
                return;
            }
        }
    }
}
