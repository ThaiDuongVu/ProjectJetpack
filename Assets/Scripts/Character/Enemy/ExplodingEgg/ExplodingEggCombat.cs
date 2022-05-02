using UnityEngine;

public class ExplodingEggCombat : EnemyCombat
{
    private ExplodingEgg _explodingEgg;

    [SerializeField] private int damage = 3;
    [SerializeField] private float explodingForce = 15f;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _explodingEgg = GetComponent<ExplodingEgg>();
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<Player>();
        player.TakeDamage(damage);
        player.KnockBack((player.transform.position - transform.position).normalized, explodingForce);
        
        _explodingEgg.Die();
    }
}
