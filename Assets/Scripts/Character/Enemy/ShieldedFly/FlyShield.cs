using UnityEngine;

public class FlyShield : Enemy
{
    [SerializeField] private int damage = 2;
    [SerializeField] private float knockBackForce = 10f;

    public override void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Player")) return;

        var player = other.transform.GetComponent<Player>();
        player.TakeDamage(damage);
        player.KnockBack((player.transform.position - transform.position).normalized, knockBackForce);
    }
}
