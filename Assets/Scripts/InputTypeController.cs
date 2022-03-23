using UnityEngine;
using UnityEngine.InputSystem;

public class InputTypeController : MonoBehaviour
{
    #region Singleton

    private static InputTypeController _inputTypeControllerInstance;

    public static InputTypeController Instance
    {
        get
        {
            if (_inputTypeControllerInstance == null)
                _inputTypeControllerInstance = FindObjectOfType<InputTypeController>();
            return _inputTypeControllerInstance;
        }
    }

    #endregion

    public InputType InputType { get; private set; } = InputType.Gamepad;

    #region Unity Event

    private void FixedUpdate()
    {
        if (Mouse.current.delta.ReadValue().magnitude > 0f) InputType = InputType.MouseKeyboard;
    }

    #endregion

    public void CheckInputType(InputAction.CallbackContext context)
    {
        InputType = context.control.device == InputSystem.devices[0] || context.control.device == InputSystem.devices[1]
            ? InputType.MouseKeyboard
            : InputType.Gamepad;
    }
}