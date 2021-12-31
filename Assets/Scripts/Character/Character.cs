using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterMovement CharacterMovement { get; private set; }

    public bool IsDead { get; set; }
    public bool IsFlipped { get; private set; }
    public bool IsStagger { get; private set; }

    public Rigidbody2D Rigidbody2D { get; private set; }
    public Animator Animator { get; private set; }
    private static int IsStaggerAnimationTrigger = Animator.StringToHash("isStagger");

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    public virtual void Awake()
    {
        CharacterMovement = GetComponent<CharacterMovement>();

        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    public virtual void Start()
    {

    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    public virtual void FixedUpdate()
    {

    }

    /// <summary>
    /// Set whether current character is flipped.
    /// </summary>
    /// <param name="value">Whether character is flipped</param>
    public void SetFlipped(bool value)
    {
        transform.localScale = new Vector2(value ? -1f : 1f, 1f);
        IsFlipped = value;
    }

    /// <summary>
    /// Knock a character back using force.
    /// </summary>
    /// <param name="direction">Direction to apply force</param>
    /// <param name="force">Force to apply</param>
    /// <param name="duration">How long to stagger for</param>
    public virtual IEnumerator Stagger(Vector2 direction, float force, float duration)
    {
        // Start staggering
        IsStagger = true;

        CharacterMovement?.StopRunningImmediate();
        Rigidbody2D.velocity = Vector2.zero;
        Rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);
        foreach (var parameter in Animator.parameters)
            if (parameter.nameHash == IsStaggerAnimationTrigger) Animator.SetBool(IsStaggerAnimationTrigger, true);

        // Wait during the time of stagger
        yield return new WaitForSeconds(duration);

        // Stop staggering
        Rigidbody2D.velocity = Vector2.zero;
        foreach (var parameter in Animator.parameters)
            if (parameter.nameHash == IsStaggerAnimationTrigger) Animator.SetBool(IsStaggerAnimationTrigger, false);

        IsStagger = false;
    }

    /// <summary>
    /// Deal damage to current character.
    /// </summary>
    /// <param name="damage">Damage to deal</param>
    public virtual void TakeDamage(float damage)
    {

    }

    /// <summary>
    /// Handle current character death.
    /// </summary>
    public virtual void Die()
    {

    }
}
