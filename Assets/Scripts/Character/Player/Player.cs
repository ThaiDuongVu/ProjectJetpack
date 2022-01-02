using System.Collections;
using UnityEngine;

public class Player : Character
{
    public bool IsControllable { get; set; } = true;

    public PlayerMovement Movement { get; private set; }
    public PlayerCombat Combat { get; private set; }
    public PlayerResources Resources { get; private set; }
    public PlayerCombo Combo { get; private set; }

    public PlayerJetpack Jetpack { get; private set; }

    [SerializeField] private Trail trailPrefab;

    [SerializeField] private ParticleSystem explosionWhitePrefab;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    public override void Awake()
    {
        base.Awake();

        Movement = GetComponent<PlayerMovement>();
        Combat = GetComponent<PlayerCombat>();
        Resources = GetComponent<PlayerResources>();
        Combo = GetComponent<PlayerCombo>();

        Jetpack = GetComponentInChildren<PlayerJetpack>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    public override void Start()
    {
        base.Start();

        Instantiate(trailPrefab, transform.position, Quaternion.identity).Target = transform;
    }

    /// <summary>
    /// Deal damage to current player.
    /// </summary>
    /// <param name="damage">Damage to deal</param>
    public override void TakeDamage(float damage)
    {
        Resources.Health -= damage;
    }

    /// <summary>
    /// Handle current player death.
    /// </summary>
    public override void Die()
    {
        if (IsDead) return;

        IsDead = true;
    }

    /// <summary>
    /// Knock a character back using force.
    /// </summary>
    /// <param name="direction">Direction to apply force</param>
    /// <param name="force">Force to apply</param>
    /// <param name="duration">How long to stagger for</param>
    public override IEnumerator Stagger(Vector2 direction, float force, float duration)
    {
        Jetpack.StopHovering();
        return base.Stagger(direction, force, duration);
    }
}
