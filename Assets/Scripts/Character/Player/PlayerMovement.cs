using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : CharacterMovement
{
    private Player _player;

    private InputManager _inputManager;

    #region Input Methods

    private void MoveOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        var direction = context.ReadValue<Vector2>() * (PlayerPrefs.GetInt("InvertAim", 0) == 0 ? 1f : -1f);
        if (direction.x < 0f) direction = Vector2.left;
        else if (direction.x > 0f) direction = Vector2.right;
        else direction = Vector2.zero;

        StartRunning(direction);
    }

    private void MoveOnCanceled(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        StopRunning();
    }

    #endregion

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle movement input
        _inputManager.Player.Move.performed += MoveOnPerformed;
        _inputManager.Player.Move.canceled += MoveOnCanceled;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    public override void Awake()
    {
        base.Awake();

        _player = GetComponent<Player>();
    }

    #endregion
}