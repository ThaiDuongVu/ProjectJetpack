using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : Character
{
    public EnemyResources EnemyResources { get; private set; }

    [SerializeField] private ParticleSystem explosionGreen;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    public override void Awake()
    {
        base.Awake();

        EnemyResources = GetComponent<EnemyResources>();
    }

    /// <summary>
    /// Deal damage to current enemy.
    /// </summary>
    /// <param name="damage">Damage to deal</param>
    public override void TakeDamage(float damage)
    {
        EnemyResources.Health -= damage;
    }

    /// <summary>
    /// Handle current enemy death.
    /// </summary>
    public override void Die()
    {
        if (IsDead) return;

        Instantiate(explosionGreen, transform.position, Quaternion.identity);
        Destroy(gameObject);

        IsDead = true;
    }
}
