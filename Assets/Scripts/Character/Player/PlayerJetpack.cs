using UnityEngine;

public class PlayerJetpack : MonoBehaviour
{
    public Player Player { get; private set; }

    public bool IsHovering { get; set; }

    [SerializeField] private Transform gunPoint;

    [Header("Hover")]
    [SerializeField] private ParticleSystem hoverMuzzle;
    private static int IsHoveringAnimationTrigger = Animator.StringToHash("isHovering");

    [Header("Fire")]
    [SerializeField] private float fireForce = 12f;
    [SerializeField] private ParticleSystem fireMuzzlePrefab;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        Player = transform.parent.GetComponent<Player>();
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        if (IsHovering) CameraShaker.Instance.Shake(CameraShakeMode.Nano);
    }

    #region Hover

    /// <summary>
    /// Player enter hovering state.
    /// </summary>
    public void StartHovering()
    {
        // Reset velocity and enable kinematic mode
        Player.Rigidbody2D.velocity = Vector2.zero;
        Player.Rigidbody2D.isKinematic = true;

        // Play hovering animation and effects
        Player.Animator.SetBool(IsHoveringAnimationTrigger, true);
        hoverMuzzle.Play();

        // Update hovering state
        IsHovering = true;
    }

    /// <summary>
    /// Player exit hovering state.
    /// </summary>
    public void StopHovering()
    {
        // Disable kinematic mode
        Player.Rigidbody2D.isKinematic = false;

        // Stop hovering animation and effects
        Player.Animator.SetBool(IsHoveringAnimationTrigger, false);
        hoverMuzzle.Stop();

        // Update hovering state
        IsHovering = false;
    }

    #endregion

    #region Fire

    /// <summary>
    /// Fire from jetpack.
    /// </summary>
    public void Fire()
    {
        // Reset vertical velocity first then apply force to fire
        Player.Rigidbody2D.velocity = new Vector2(Player.Rigidbody2D.velocity.x, 0f);
        Player.Rigidbody2D.AddForce(Vector2.up * fireForce, ForceMode2D.Impulse);

        // Play fire effects and shake the camera
        Instantiate(fireMuzzlePrefab, gunPoint.position, Quaternion.identity);
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    #endregion
}
