using UnityEngine;

public class ToxicRatCombat : EnemyCombat
{
    private ToxicRat _toxicRat;

    [SerializeField] private int damage = 1;
    [SerializeField] private float knockBackForce = 10f;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _toxicRat = GetComponent<ToxicRat>();
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Player")) return;

        var player = other.transform.GetComponent<Player>();
        player.TakeDamage(damage);
        player.KnockBack(new Vector2((player.transform.position - transform.position).normalized.x, 1f).normalized, knockBackForce);
    }
}
