using UnityEngine;
using UnityEngine.InputSystem;

public class InputTypeController : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible

    #region Singleton

    private static InputTypeController inputTypeControllerInstance;

    public static InputTypeController Instance
    {
        get
        {
            if (inputTypeControllerInstance == null) inputTypeControllerInstance = FindObjectOfType<InputTypeController>();
            return inputTypeControllerInstance;
        }
    }

    #endregion

    public InputType InputType { get; private set; } = InputType.Gamepad;

    /// <summary>
    /// Switch input type based on current input context.
    /// </summary>
    /// <param name="context">Input context</param>
    public void CheckInputType(InputAction.CallbackContext context)
    {
        InputType = context.control.device == InputSystem.devices[0] || context.control.device == InputSystem.devices[1] ? InputType.MouseKeyboard : InputType.Gamepad;
    }
}
