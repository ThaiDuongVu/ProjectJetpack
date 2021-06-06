using UnityEngine;

public class Humanoid : MonoBehaviour
{
    private Enemy enemy;
    private float lookAccuracy;
    private const float MinAccuracy = 0.025f;
    private const float MaxAccuracy = 0.15f;

    private const float MinFireRate = 0.5f;
    private const float MaxFireRate = 2f;
    private float timer;
    private float timerMax;

    private new Rigidbody2D rigidbody2D;
    private const float MovementSpeed = 5f;

    [SerializeField] private Transform firePoint;
    [SerializeField] private Bullet bulletPrefab;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        // Generate a random aim accuracy for this enemy
        lookAccuracy = Random.Range(MinAccuracy, MaxAccuracy);
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        if (enemy.IsDead) return;

        LookAt(Player.Instance.transform);
        if (IsTimer())
        {
            Fire();
            ResetTimer();
        }
    }

    /// <summary>
    /// Look at a target (usually the player) in the game world
    /// </summary>
    /// <param name="target">Target to look at</param>
    private void LookAt(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.up = Vector2.Lerp(transform.up, direction, lookAccuracy);
    }

    /// <summary>
    /// Walk at a direction.
    /// </summary>
    /// <param name="direction">Direction to walk at</param>
    private void Walk(Vector2 direction)
    {
        rigidbody2D.MovePosition((Vector2)rigidbody2D.position + direction * MovementSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Fire current weapon.
    /// </summary>
    private void Fire()
    {
        Instantiate(bulletPrefab, firePoint.transform.position, transform.rotation);
    }

    /// <summary>
    /// Reset fire timer.
    /// </summary>
    private void ResetTimer()
    {
        timer = 0f;
        timerMax = Random.Range(MinFireRate, MaxFireRate);
    }

    /// <summary>
    /// Check if fire timer is reached.
    /// </summary>
    /// <return>Whether fire timer is reached</return>
    private bool IsTimer()
    {
        timer += Time.fixedDeltaTime;
        return timer >= timerMax;
    }
}
