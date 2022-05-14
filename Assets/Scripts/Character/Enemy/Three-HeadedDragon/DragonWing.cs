public class DragonWing : Enemy
{
    private DragonHead _dragonHead;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _dragonHead = GetComponentInParent<DragonHead>();
    }

    #endregion

    public override void Die()
    {
        _dragonHead.WingsCount--;
        AudioController.Instance.Play(AudioVariant.Explode1);

        base.Die();
    }
}
