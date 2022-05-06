using UnityEngine;

public class Fireball : Enemy
{
    public Transform Target { get; set; }

    [SerializeField] private float velocity = 4f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockBackForce = 15f;
    private Vector2 _direction;

    #region Unity Event

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _direction = (Target.position - transform.position).normalized;
        Rigidbody2D.MovePosition(Rigidbody2D.position + _direction * (velocity * Time.fixedDeltaTime));
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
            player.KnockBack(_direction, knockBackForce);
        }
		
		CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Die();
    }
}
