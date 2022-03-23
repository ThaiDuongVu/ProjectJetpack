using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    private Player _player;
    public float force = 500f;

    private InputManager _inputManager;

    #region Input Methods

    #endregion

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

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

    public override void Start()
    {
        base.Start();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    #endregion
}