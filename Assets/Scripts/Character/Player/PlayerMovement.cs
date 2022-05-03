using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : CharacterMovement
{
    private Player _player;
    private InputManager _inputManager;

    #region Input Methods

    private void MoveOnPerformed(InputAction.CallbackContext context)
    {
        if (!_player.IsControllable || GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        var direction = context.ReadValue<Vector2>().normalized * (PlayerPrefs.GetInt("InvertAim", 0) == 0 ? 1f : -1f);

        if (direction.y <= -0.7f) StartCoroutine(DropDownPlatform());

        if (direction.x < 0f) direction = Vector2.left;
        else if (direction.x > 0f) direction = Vector2.right;
        else
        {
            StopRunning();
            return;
        }

        if (direction.x * CurrentDirection.x < 0f) StopRunningImmediate();
        StartRunning(direction);
    }

    private void MoveOnCanceled(InputAction.CallbackContext context)
    {
        if (!_player.IsControllable || GameController.Instance.State == GameState.Paused) return;
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

    private IEnumerator DropDownPlatform()
    {
        if (_player.IsGrounded && _player.GroundPlatform && !_player.GroundPlatform.transform.CompareTag("BasePlatform"))
        {
            var tempGround = _player.GroundPlatform;
            Physics2D.IgnoreCollision(_player.Collider2D, tempGround, true);
            yield return new WaitForSeconds(0.5f);
            Physics2D.IgnoreCollision(_player.Collider2D, tempGround, false);
        }
    }
}