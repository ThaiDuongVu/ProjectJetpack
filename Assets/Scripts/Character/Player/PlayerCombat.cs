using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    private Player _player;

    [SerializeField] private Transform jetpackHolder;
    private PlayerJetpack[] _jetpackPrefabs;
    public const string JetpackKey = "PlayerJetpack";
    [SerializeField] private PlayerJetpack blueJetpackPrefab;

    public PlayerJetpack PlayerJetpack { get; set; }

    private InputManager _inputManager;

    #region Input Methods

    private void JumpOnStarted(InputAction.CallbackContext context)
    {
        if (!_player.IsControllable || GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        if (!PlayerJetpack) return;

        if (PlayerJetpack.IsHovering) PlayerJetpack.SetHovering(false);
        PlayerJetpack.Jump();
    }

    private void HoverOnStarted(InputAction.CallbackContext context)
    {
        if (!_player.IsControllable || GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        if (!PlayerJetpack) return;
        PlayerJetpack.SetHovering(true);
    }

    private void HoverOnCanceled(InputAction.CallbackContext context)
    {
        if (!_player.IsControllable || GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        if (!PlayerJetpack) return;
        PlayerJetpack?.SetHovering(false);
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
        _jetpackPrefabs = Resources.LoadAll<PlayerJetpack>("Players/Jetpacks");
    }

    public override void Start()
    {
        base.Start();

        Invoke(nameof(InitJetpack), 0.1f);
    }

    #endregion

    private void InitJetpack()
    {
        PlayerJetpack = Instantiate(_jetpackPrefabs[PlayerPrefs.GetInt(JetpackKey, 0)], jetpackHolder.position, Quaternion.identity);
        PlayerJetpack.transform.parent = jetpackHolder;
        PlayerJetpack.transform.localScale = Vector2.one;
        PlayerJetpack.Player = _player;
        _player.crosshair = PlayerJetpack.crosshair;
    }

    public void UpdateJetpack()
    {
        Destroy(PlayerJetpack.gameObject);
        InitJetpack();
    }
}
