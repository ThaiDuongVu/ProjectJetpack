using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public Collider2D Collider2D { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }

    public CharacterMovement CharacterMovement { get; private set; }
    public CharacterCombat CharacterCombat { get; private set; }
    public CharacterResources CharacterResources { get; private set; }

    public bool IsDead { get; set; }
    public bool IsFlipped { get; set; }

    [SerializeField] private SpriteRenderer mainSprite;

    #region Unity Event

    public virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Collider2D = GetComponent<Collider2D>();
        Rigidbody2D = GetComponent<Rigidbody2D>();

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

    public void Stagger(Vector2 direction, float force)
    {
        if (!Rigidbody2D) return;

        CharacterMovement?.StopRunningImmediate();
        
        Rigidbody2D.velocity = Vector2.zero;
        Rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);
    }
}