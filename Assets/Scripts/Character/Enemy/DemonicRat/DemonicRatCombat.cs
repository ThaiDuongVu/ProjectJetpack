using UnityEngine;

public class DemonicRatCombat : EnemyCombat
{
    [SerializeField] private ParticleSystem bloodSplashRedPrefab;
    private static readonly int AttackAnimationTrigger = Animator.StringToHash("attack");

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

        _demonicRat.Animator.SetTrigger(AttackAnimationTrigger);

        var player = other.transform.GetComponent<Player>();
        player.TakeDamage(_demonicRat.DemonicRatResources.damage);
        player.Stagger(_demonicRat.DemonicRatMovement.CurrentDirection);

        Instantiate(bloodSplashRedPrefab, player.transform.position, Quaternion.identity).transform.up = _demonicRat.DemonicRatMovement.CurrentDirection;
        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
        _demonicRat.StopRushing();
    }
}
