public class SpiderEye : Enemy
{
    public OneEyeSpider OneEyeSpider { get; set; }

    #region Unity Event

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!OneEyeSpider) return;

        transform.position = OneEyeSpider.transform.position;
    }

    #endregion

    public override void TakeDamage(int damage)
    {
        OneEyeSpider.TakeDamage(damage);
    }

    public override void Die()
    {
        OneEyeSpider.Die();

        base.Die();
    }
}
