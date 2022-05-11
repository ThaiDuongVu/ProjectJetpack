using UnityEngine;

public class Fireball : Enemy
{
    public Transform Target { get; set; }

    [SerializeField] protected float velocity = 4f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float knockBackForce = 15f;
    protected Vector2 direction;

    #region Unity Event

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!Target) Die();

        direction = (Target.position - transform.position).normalized;
        Rigidbody2D.MovePosition(Rigidbody2D.position + direction * (velocity * Time.fixedDeltaTime));
    }

    #endregion

    public override void Die()
    {
        base.Die();

        AudioController.Instance.Play(AudioVariant.Explode1);
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);

        if (other.transform.CompareTag("Player"))
        {
            var player = other.transform.GetComponent<Player>();

            player.TakeDamage(damage);
            player.KnockBack(direction, knockBackForce);
        }

        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Die();
    }
}
