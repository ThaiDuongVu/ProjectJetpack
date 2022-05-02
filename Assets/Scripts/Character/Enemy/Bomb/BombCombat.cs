using UnityEngine;

public class BombCombat : EnemyCombat
{
    private Bomb _bomb;

    [SerializeField] private int damage = 3;
    [SerializeField] private float explodingForce = 15f;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _bomb = GetComponent<Bomb>();
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Player")) return;

        var player = other.transform.GetComponent<Player>();
        player.TakeDamage(damage);
        player.KnockBack((player.transform.position - transform.position).normalized, explodingForce);
        
        _bomb.Die();
    }
}
