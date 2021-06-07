using UnityEngine;

public class Bullet : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    protected float speed = 40f;
    protected float damage = 1f;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        Fly();
    }

    /// <summary>
    /// Bullet fly through game world.
    /// </summary>
    private void Fly()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + (Vector2)transform.up * speed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Unity Event function.
    /// Handle collision with other objects.
    /// </summary>
    /// <param name="other">Collision to check</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemy"))
            (other.transform.GetComponent<Enemy>() as IDamageable).TakeDamage(damage, transform.up);
        else if (other.transform.CompareTag("Player") && !Player.Instance.IsDashing)
            (Player.Instance as IDamageable).TakeDamage(damage, transform.up);

        Destroy(gameObject);
    }
}
