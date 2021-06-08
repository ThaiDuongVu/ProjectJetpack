using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    private const float FollowInterpolationRatio = 0.05f;
    private const float YOffset = 10f;
    public Transform followTarget;

    private float lookVelocity;
    public const float LookSensitivity = 1f;
    private const float MaxLookVelocity = 6f;
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
        Follow(followTarget);
        Rotate();
    }

    /// <summary>
    /// Follow a target in scene.
    /// </summary>
    /// <param name="target">Target in scene</param>
    private void Follow(Transform target)
    {
        if (!target) return;

        // Lerp to target position
        Vector2 targetPosition = target.position;

        transform.position = Vector3.Lerp(transform.position,
            new Vector3(targetPosition.x, targetPosition.y, -10f) + transform.up * YOffset,
            FollowInterpolationRatio);
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
        // Rotate camera
        transform.Rotate(Vector3.forward, lookVelocity * (PlayerPrefs.GetInt("InvertLook", 0) == 0 ? 1f : -1f) * Time.timeScale, Space.World);
    }
}