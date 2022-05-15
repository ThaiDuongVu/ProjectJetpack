using UnityEngine;

public class Lava : Enemy
{
    [SerializeField] private int damage = 2;
    [SerializeField] private float knockBackForce = 10f;

    #region Unity Event

    #endregion

    public override void OnCollisionEnter2D(Collision2D other)
    {
        // base.OnCollisionEnter2D(other);

        if (!other.transform.CompareTag("Player")) return;

        var player = other.transform.GetComponent<Player>();
        StartCoroutine(player.PlayerMovement.DropDownPlatform());
        player.TakeDamage(damage);
        player.KnockBack(Vector2.down, knockBackForce);
    }
}
