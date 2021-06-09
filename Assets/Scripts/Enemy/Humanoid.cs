using UnityEngine;

public class Humanoid : MonoBehaviour
{
    private Enemy enemy;
    private float lookAccuracy;
    private const float MinAccuracy = 0.025f;
    private const float MaxAccuracy = 0.15f;

    private new Rigidbody2D rigidbody2D;
    private const float MovementSpeed = 5f;

    [SerializeField] private Transform firePoint;
    [SerializeField] private Bullet bulletPrefab;

    private Vector2 currentDirection;

    private TimeToAttack timeToAttack;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        timeToAttack = GetComponent<TimeToAttack>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        // Generate a random aim accuracy for this enemy
        lookAccuracy = Random.Range(MinAccuracy, MaxAccuracy);

        timeToAttack.OnTimerReset = () => { currentDirection = (new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized; };
        timeToAttack.MinAttackRate = 1f;
        timeToAttack.MaxAttackRate = 3f;
        timeToAttack.ResetTimer();
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        if (enemy.IsDead) return;

        LookAt(Player.Instance.transform);
        Walk(currentDirection);

        if (timeToAttack.IsTimer())
        {
            Fire();
            timeToAttack.ResetTimer();
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
    /// Unity Event function.
    /// Handle trigger collision with other objects.
    /// </summary>
    /// <param name="other">Collider to check</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Enemy"))
        {
            currentDirection = -currentDirection;
        }
        else if (other.CompareTag("Player"))
        {
            currentDirection = -Player.Instance.transform.up;
        }
    }
}
