using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D { get; private set; }

    public float velocity = 20f;
    public Vector2 direction;

    public float damage = 1f;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        Rigidbody2D.velocity = direction * velocity;
    }

    /// <summary>
    /// Unity Event function.
    /// Handle trigger collision enter with other objects.
    /// </summary>
    /// <param name="other">Other collider to handle</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) other.GetComponent<Enemy>().TakeDamage(damage);
        
        Destroy(gameObject);
    }
}
