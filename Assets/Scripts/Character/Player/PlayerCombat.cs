using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public Player Player { get; private set; }

    private InputManager inputManager;

    /// <summary>
    /// Unity Event function.
    /// On current object enabled.
    /// </summary>
    private void OnEnable()
    {
        inputManager = new InputManager();

        // Handle hover input
        inputManager.Player.Hover.started += HoverOnStarted;
        inputManager.Player.Hover.canceled += HoverOnCanceled;

        // Handle fire input
        inputManager.Player.Fire.started += FireOnStarted;

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
    /// On hover input started.
    /// </summary>
    /// <param name="context">Input context</param>
    private void HoverOnStarted(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused || !Player.IsControllable || Player.IsStagger) return;
        InputTypeController.Instance.CheckInputType(context);

        Player.Jetpack.StartHovering();
    }

    /// <summary>
    /// On hover input canceled.
    /// </summary>
    /// <param name="context">Input context</param>
    private void HoverOnCanceled(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused || !Player.IsControllable || Player.IsStagger) return;
        InputTypeController.Instance.CheckInputType(context);

        Player.Jetpack.StopHovering();
    }

    /// <summary>
    /// On fire input started.
    /// </summary>
    /// <param name="context">Input context</param>
    private void FireOnStarted(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused || !Player.IsControllable || Player.IsStagger) return;
        InputTypeController.Instance.CheckInputType(context);

        if (Player.Jetpack.IsHovering) Player.Jetpack.StopHovering();
        Player.Jetpack.Fire();
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
}
