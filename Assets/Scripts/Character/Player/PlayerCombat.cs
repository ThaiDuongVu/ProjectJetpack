using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    private Player _player;

    [SerializeField] private Transform jetpackHolder;
    [SerializeField] private PlayerJetpack blueJetpackPrefab;

    public PlayerJetpack PlayerJetpack { get; set; }

    private InputManager _inputManager;

    #region Input Methods

    private void JumpOnStarted(InputAction.CallbackContext context)
    {
        if (!_player.IsControllable || GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        if (PlayerJetpack.IsHovering) PlayerJetpack.SetHovering(false);
        PlayerJetpack.Jump();
    }

    private void HoverOnStarted(InputAction.CallbackContext context)
    {
        if (!_player.IsControllable || GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        PlayerJetpack.SetHovering(true);
    }

    private void HoverOnCanceled(InputAction.CallbackContext context)
    {
        if (!_player.IsControllable || GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        PlayerJetpack.SetHovering(false);
    }

    #endregion

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle jump input
        _inputManager.Player.Jump.started += JumpOnStarted;

        // Handle hover input
        _inputManager.Player.Hover.started += HoverOnStarted;
        _inputManager.Player.Hover.canceled += HoverOnCanceled;

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

        PlayerJetpack = Instantiate(blueJetpackPrefab, jetpackHolder.position, Quaternion.identity);
        PlayerJetpack.transform.parent = jetpackHolder;
        PlayerJetpack.Player = _player;
        _player.crosshair = PlayerJetpack.crosshair;
    }

    #endregion
}
