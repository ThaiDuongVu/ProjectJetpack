using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJetpack : MonoBehaviour
{
    public Player Player { get; private set; }

    public bool IsHovering { get; set; }

    [Header("Hover")]
    [SerializeField] private float hoverForce = 30f;
    [SerializeField] private float hoverFuelDepletionRate = 5f;
    public ParticleSystem jetpackMuzzle;

    private InputManager inputManager;

    /// <summary>
    /// Unity Event function.
    /// On current object enabled.
    /// </summary>
    private void OnEnable()
    {
        inputManager = new InputManager();

        // Handle hover input
        inputManager.Player.Hover.performed += HoverOnPerformed;
        inputManager.Player.Hover.canceled += HoverOnCanceled;

        inputManager.Enable();
    }

    /// <summary>
    /// Unity Event function.
    /// On current object disabled.
    /// </summary>
    private void OnDisable()
    {
        inputManager.Disable();
    }

    #region Input Methods

    /// <summary>
    /// On hover input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void HoverOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused || !Player.IsControllable || Player.IsStagger) return;
        InputTypeController.Instance.CheckInputType(context);

        StartHovering();
    }

    /// <summary>
    /// On hover input canceled.
    /// </summary>
    /// <param name="context">Input context</param>
    private void HoverOnCanceled(InputAction.CallbackContext context)
    {
        StopHovering();
    }

    #endregion

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        Player = transform.parent.GetComponent<Player>();
    }

    private void Start()
    {

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

            CameraShaker.Instance.Shake(CameraShakeMode.Nano);
        }
    }

    #region Hover Methods

    /// <summary>
    /// Player enter hovering state using jetpack.
    /// </summary>
    public void StartHovering()
    {
        Player.Rigidbody2D.velocity = Vector2.zero;
        Player.Animator.SetBool("isHovering", true);

        IsHovering = true;
        jetpackMuzzle.Play();
    }

    /// <summary>
    /// Player exit hovering state using jetpack.
    /// </summary>
    public void StopHovering()
    {
        Player.Animator.SetBool("isHovering", false);

        IsHovering = false;
        jetpackMuzzle.Stop();
    }

    #endregion
}
