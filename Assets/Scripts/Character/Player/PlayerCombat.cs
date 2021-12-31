using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public Player Player { get; private set; }

    [Header("Fire")]
    public float fireFore = 12f;

    private InputManager inputManager;

    /// <summary>
    /// Unity Event function.
    /// On current object enabled.
    /// </summary>
    private void OnEnable()
    {
        inputManager = new InputManager();

        // Handle fire input
        inputManager.Player.Fire.performed += FireOnPerformed;

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
    private void FireOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused || !Player.IsControllable || Player.IsStagger) return;
        if (Player.Jetpack.IsHovering) return;
        InputTypeController.Instance.CheckInputType(context);

        Fire();
    }

    #endregion

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        Player = GetComponent<Player>();
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {

    }

    /// <summary>
    /// Fire current jetpack.
    /// </summary>
    private void Fire()
    {
        Player.Movement.Jump();

        // TODO: Decrease fuel
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }
}
