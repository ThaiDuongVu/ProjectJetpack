public class ExplodingEgg : Enemy
{
    public override void Die()
    {
        base.Die();

        AudioController.Instance.Play(AudioVariant.Explode2);
    }
}
