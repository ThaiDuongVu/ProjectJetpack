public class DragonHead : Enemy
{
    private int _wingsCount;
    public int WingsCount
    {
        get => _wingsCount;
        set
        {
            _wingsCount = value;
            if (value <= 0) Die();
        }
    }

    private Dragon _dragon;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        WingsCount = GetComponentsInChildren<DragonWing>().Length;
        _dragon = GetComponentInParent<Dragon>();
    }

    #endregion

    public override void TakeDamage(int damage)
    {
        // base.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();

        _dragon.DragonHeadsCount--;
        AudioController.Instance.Play(AudioVariant.Explode2);
    }
}
