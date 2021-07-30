using UnityEngine;
using UnityEngine.InputSystem;

public class InputTypeController : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible

    #region Singleton

    private static InputTypeController instance;

    public static InputTypeController Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<InputTypeController>();

            return instance;
        }
    }

    #endregion

    public InputType InputType { get; set; } = InputType.Gamepad;

    /// <summary>
    /// Switch input type based on current input context.
    /// </summary>
    /// <param name="context">Input context</param>
    public void CheckInputType(InputAction.CallbackContext context)
    {
        InputType = context.control.device == InputSystem.devices[0] || context.control.device == InputSystem.devices[1] ? InputType.MouseKeyboard : InputType.Gamepad;
    }
}
