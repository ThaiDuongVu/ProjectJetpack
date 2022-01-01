using System.Threading.Tasks;
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
        inputManager.Player.Hover.performed += HoverOnPerformed;
        inputManager.Player.Hover.canceled += HoverOnCanceled;

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
    private void HoverOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused || !Player.IsControllable || Player.IsStagger) return;
        if (Player.Resources.Fuel <= 0f) return;
        InputTypeController.Instance.CheckInputType(context);

        Player.Jetpack.StartHovering();
    }

    /// <summary>
    /// On hover input canceled.
    /// </summary>
    /// <param name="context">Input context</param>
    private void HoverOnCanceled(InputAction.CallbackContext context)
    {
        Player.Jetpack.StopHovering();
    }

    /// <summary>
    /// On hover input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void FireOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused || !Player.IsControllable || Player.IsStagger) return;
        if (Player.Resources.Fuel < Player.Jetpack.fireFuelConsumptionPerShot) return;
        InputTypeController.Instance.CheckInputType(context);

        Player.Jetpack.StopHovering();
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
