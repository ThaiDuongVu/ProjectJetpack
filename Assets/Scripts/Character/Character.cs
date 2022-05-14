using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public Collider2D Collider2D { get; private set; }

    private CharacterMovement _characterMovement;
    private CharacterResources _characterResources;

    private bool _isDead;
    [SerializeField] protected ParticleSystem explosionPrefab;

    public bool IsFlipped { get; private set; }
    public SpriteRenderer mainSprite;

    [Header("Ground Properties")]
    [SerializeField] private float groundRaycastDistance = 0.55f;
    public virtual bool IsGrounded { get; protected set; }
    public Collider2D GroundPlatform { get; private set; }
    private static readonly int FallAnimationTrigger = Animator.StringToHash("isFalling");

    [Header("Edge Properties")]
    [SerializeField] private Transform[] edgePoints;
    [SerializeField] private float edgeRaycastDistance = 0.55f;
    private readonly string[] _levelLayers = { "Levels" };
    protected bool IsEdged { get; private set; }

    #region Unity Event

    public virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();

        _characterMovement = GetComponent<CharacterMovement>();
        _characterResources = GetComponent<CharacterResources>();
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
        _characterResources.Health -= damage;
    }

    public virtual void Die()
    {
        if (_isDead) return;

        _isDead = true;

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
        if (_characterMovement) _characterMovement.StopRunningImmediate();

        Rigidbody2D.velocity = Vector2.zero;
        Rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);
    }

    protected virtual void DetectGrounded()
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

    protected void DetectEdge()
    {
        IsEdged = true;

        if (edgePoints.Any(point => Physics2D.Raycast(point.position, Vector2.down, edgeRaycastDistance,
                LayerMask.GetMask(_levelLayers))))
        {
            IsEdged = false;
        }
    }
}
