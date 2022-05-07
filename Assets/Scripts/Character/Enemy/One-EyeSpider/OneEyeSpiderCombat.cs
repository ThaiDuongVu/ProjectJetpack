using UnityEngine;

public class OneEyeSpiderCombat : EnemyCombat
{
    private OneEyeSpider _oneEyeSpider;

    [SerializeField] private int damage = 3;
    [SerializeField] private float knockBackForce = 30f;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _oneEyeSpider = GetComponent<OneEyeSpider>();
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Player")) return;

        var player = other.transform.GetComponent<Player>();
        player.TakeDamage(damage);
        player.KnockBack(new Vector2(_oneEyeSpider.Direction.x, 1f).normalized, knockBackForce);
    }
}
