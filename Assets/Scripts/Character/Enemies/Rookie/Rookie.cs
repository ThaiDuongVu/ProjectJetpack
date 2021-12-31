using UnityEngine;

public class Rookie : Enemy
{
    public RookieResources Resources { get; private set; }

    [SerializeField] private ParticleSystem bloodSplashPrefab;

    /// <summary>
    /// Unity Event function.
    /// Gte component references.
    /// </summary>
    public override void Awake()
    {
        base.Awake();

        Resources = GetComponent<RookieResources>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    public override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    /// <summary>
    /// Unity Event function.
    /// Handle collision enter with other objects.
    /// </summary>
    /// <param name="other">Other collider to handle</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            var player = other.transform.GetComponent<Player>();
            // Deal damage to player on collided
            player.TakeDamage(Resources.damage);

            // Some effects on player damage
            StartCoroutine(player.Stagger(((IsFlipped ? Vector2.left : Vector2.right) + Vector2.up).normalized, Resources.damageKnockBackForce, Resources.damageStaggerDuration));
            Instantiate(bloodSplashPrefab, player.transform.position, Quaternion.identity).transform.localScale = new Vector2(IsFlipped ? 1f : -1f, 1f);
            
            CameraShaker.Instance.Shake(CameraShakeMode.Normal);
        }
    }
}