using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform followTarget;
    public static Vector3 DefaultOffset { get; set; } = new Vector3(0f, 0f, 0f);
    public Vector3 Offset { get; set; } = DefaultOffset;
    private const float InterpolationRatio = 0.3f;

    private float lookVelocity;
    public const float LookSensitivity = 1f;
    private const float MaxLookVelocity = 5f;
    private const float LookDeadZone = 0.1f;

    private InputManager inputManager;

    /// <summary>
    /// Unity Event function.
    /// Initialize input handler on object enabled.
    /// </summary>
    private void OnEnable()
    {
        inputManager = new InputManager();

        // Handle look input
        inputManager.Player.Look.performed += LookOnPerformed;
        inputManager.Player.Look.canceled += LookOnCanceled;

        inputManager.Enable();
    }

    #region Input Methods

    /// <summary>
    /// On look input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void LookOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance.State != GameState.Started) return;

        lookVelocity = context.ReadValue<Vector2>().x * LookSensitivity;
    }

    /// <summary>
    /// On look input canceled.
    /// </summary>
    /// <param name="context">Input context</param>
    private void LookOnCanceled(InputAction.CallbackContext context)
    {
        lookVelocity = 0f;
    }

    #endregion

    /// <summary>
    /// Unity Event function.
    /// Disable input handling on object disabled.
    /// </summary>
    private void OnDisable()
    {
        inputManager.Disable();
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        Follow();
        Rotate();
    }

    /// <summary>
    /// Follow a target in top-down perspective.
    /// </summary>
    private void Follow()
    {
        if (!followTarget) return;

        Vector3 followPosition = new Vector3(followTarget.position.x, 25f, followTarget.position.z);
        Offset = new Vector3(Offset.x, 0f, Offset.z);
        transform.position = Vector3.Lerp(transform.position, followPosition - Offset, InterpolationRatio);
    }

    /// <summary>
    /// Rotate camera to input direction.
    /// </summary>
    private void Rotate()
    {
        // Handle look dead zone
        if (Mathf.Abs(lookVelocity) <= LookDeadZone) return;

        // Clamp look velocity for mouse input
        lookVelocity = Mathf.Clamp(lookVelocity, -MaxLookVelocity, MaxLookVelocity);
        transform.Rotate(Vector3.up, -lookVelocity * (PlayerPrefs.GetInt("InvertLook", 0) == 0 ? 1f : -1f) * Time.timeScale, Space.World);
    }
}
