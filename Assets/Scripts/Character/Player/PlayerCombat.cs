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

        // Handle dash input
        inputManager.Player.Dash.started += DashOnStarted;

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
    /// On dash input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void DashOnStarted(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused || !Player.IsControllable || Player.IsStagger) return;
        InputTypeController.Instance.CheckInputType(context);


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
    /// Raycast at current arrow direction to check for enemies.
    /// </summary>
    private void Aim()
    {

    }

    #region Dash

    /// <summary>
    /// Player enter dashing state.
    /// </summary>
    private void StartDashing()
    {

    }

    /// <summary>
    /// Player exit dashing state.
    /// </summary>
    private void StopDashing()
    {

    }

    /// <summary>
    /// Player perform dashing move.
    /// </summary>
    private void Dash()
    {

    }

    #endregion
}
