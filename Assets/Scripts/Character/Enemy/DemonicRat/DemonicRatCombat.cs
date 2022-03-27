using UnityEngine;

public class DemonicRatCombat : EnemyCombat
{
    private DemonicRat _demonicRat;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _demonicRat = GetComponent<DemonicRat>();
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Player")) return;

        var player = other.transform.GetComponent<Player>();
        player.TakeDamage(_demonicRat.DemonicRatResources.damage);
        
        player.Stagger((_demonicRat.Direction + Vector2.up).normalized, _demonicRat.DemonicRatResources.staggerForce);
        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
    }
}
