using UnityEngine;

public class PlayerJetpack : MonoBehaviour
{
    public Player Player { get; private set; }

    public bool IsHovering { get; set; }

    [Header("Hover")]
    [SerializeField] private float hoverForce = 30f;
    [SerializeField] private float hoverFuelDepletionRate = 5f;
    private ParticleSystem hoverMuzzle;
    private static int IsHoveringAnimationTrigger = Animator.StringToHash("isHovering");

    [Header("Fire")]
    public float fireFore = 12f;
    [SerializeField] private ParticleSystem fireMuzzle;
    public float fireFuelConsumptionPerShot = 10f;
    public Transform gunPoint;

    [SerializeField] private Bullet bulletPrefab;

    private bool canFireAutomatic = true;
    private const int FireAutomaticFireRate = 250;
    private float fireAutomaticTimer;
    

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        Player = transform.parent.GetComponent<Player>();
        hoverMuzzle = GetComponentInChildren<ParticleSystem>();
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        if (IsHovering)
        {
            Player.Rigidbody2D.AddForce(Vector2.up * hoverForce);
            Player.Resources.Fuel -= hoverFuelDepletionRate * Time.fixedDeltaTime;

            FireAutomatic();

            if (Player.Resources.Fuel <= 0f) StopHovering();
        }

        if (fireAutomaticTimer > 0f) fireAutomaticTimer -= Time.fixedDeltaTime;
        else canFireAutomatic = true;
    }

    #region Hover Methods

    /// <summary>
    /// Player enter hovering state using jetpack.
    /// </summary>
    public void StartHovering()
    {
        Player.Rigidbody2D.velocity = Vector2.zero;
        Player.Animator.SetBool(IsHoveringAnimationTrigger, true);

        IsHovering = true;
        hoverMuzzle.Play();
    }

    /// <summary>
    /// Player exit hovering state using jetpack.
    /// </summary>
    public void StopHovering()
    {
        Player.Animator.SetBool(IsHoveringAnimationTrigger, false);

        IsHovering = false;
        hoverMuzzle.Stop();
    }

    #endregion

    /// <summary>
    /// Fire current jetpack.
    /// </summary>
    public void Fire()
    {
        Player.Movement.Jump();
        Player.Resources.Fuel -= fireFuelConsumptionPerShot;

        // TODO: Fire one bullet burst from gun point

        Instantiate(fireMuzzle, gunPoint.position, Quaternion.identity);
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    /// <summary>
    /// Fire current jetpack in automatic mode.
    /// </summary>
    public void FireAutomatic()
    {
        if (!canFireAutomatic) return;

        Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity).direction = new Vector2(Random.Range(-0.15f, 0.15f), -1f).normalized;
        CameraShaker.Instance.Shake(CameraShakeMode.Nano);

        canFireAutomatic = false;
        fireAutomaticTimer = 1f / FireAutomaticFireRate;
    }
}
