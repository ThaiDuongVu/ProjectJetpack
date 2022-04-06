using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public Collider2D Collider2D { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }

    public CharacterMovement CharacterMovement { get; private set; }
    public CharacterCombat CharacterCombat { get; private set; }
    public CharacterPathfinder CharacterPathfinder { get; private set; }
    public CharacterResources CharacterResources { get; private set; }

    [Header("Position Properties")]
    public Vector2 minPosition;
    public Vector2 maxPosition;

    public bool IsDead { get; set; }
    public bool IsFlipped { get; set; }

    public bool IsStagger { get; set; }
    private static readonly int StaggerAnimationTrigger = Animator.StringToHash("isStagger");
    [Header("Stagger Properties")]
    [SerializeField] private float staggerDistance = 1.2f;
    [SerializeField] private float staggerEpsilon = 0.2f;
    private const float StaggerInterpolationRatio = 0.2f;
    private Vector2 _staggerPosition;

    [Header("")]
    public SpriteRenderer mainSprite;

    #region Unity Event

    public virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Collider2D = GetComponent<Collider2D>();
        Rigidbody2D = GetComponent<Rigidbody2D>();

        CharacterMovement = GetComponent<CharacterMovement>();
        CharacterCombat = GetComponent<CharacterCombat>();
        CharacterPathfinder = GetComponent<CharacterPathfinder>();
        CharacterResources = GetComponent<CharacterResources>();
    }

    public virtual void Start()
    {
    }

    public virtual void FixedUpdate()
    {
        if (IsStagger)
        {
            transform.position = Vector2.Lerp(transform.position, _staggerPosition, StaggerInterpolationRatio);
            if (Vector2.Distance(transform.position, _staggerPosition) <= staggerEpsilon) SetStagger(false);
        }

        if (Rigidbody2D) Rigidbody2D.velocity = Vector2.zero;
    }

    public virtual void Update()
    {
    }

    #endregion

    public virtual void SetFlipped(bool value)
    {
        mainSprite.transform.localScale = new Vector2(value ? -1f : 1f, 1f);
        IsFlipped = value;
    }

    #region Damage & Death

    public virtual void TakeDamage(int damage)
    {
        CharacterResources.Health -= damage;
    }

    public virtual void Die()
    {
    }

    #endregion

    private void SetStagger(bool value)
    {
        IsStagger = value;
        Animator.SetBool(StaggerAnimationTrigger, value);
    }

    public void Stagger(Vector2 direction)
    {
        _staggerPosition = (Vector2)transform.position + direction * staggerDistance;
        SetStagger(true);
    }
}