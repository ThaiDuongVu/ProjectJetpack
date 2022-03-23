using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameController : MonoBehaviour
{
    #region Singleton

    private static GameController _gameControllerInstance;

    public static GameController Instance
    {
        get
        {
            if (_gameControllerInstance == null) _gameControllerInstance = FindObjectOfType<GameController>();
            return _gameControllerInstance;
        }
    }

    #endregion

    public GameState State { get; private set; } = GameState.InProgress;

    [Header("Menus")] [SerializeField] private Canvas mainUI;
    [SerializeField] private Menu pauseMenu;
    [SerializeField] private Menu gameOverMenu;

    [Header("UI Message")]
    [SerializeField] private GameObject uiMessage;

    private TMP_Text _uiMessageText;

    private InputManager _inputManager;

    #region Input Methods

    private void EscapeOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);

        if (State == GameState.InProgress) Pause();
    }

    #endregion

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle game pause input
        _inputManager.Game.Escape.performed += EscapeOnPerformed;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    private void Awake()
    {
        _uiMessageText = uiMessage.GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        EffectsController.Instance.SetDepthOfField(false);
        EffectsController.Instance.SetChromaticAberration(false);
        EffectsController.Instance.SetVignetteIntensity(EffectsController.DefaultVignetteIntensity);

        uiMessage.gameObject.SetActive(false);
        mainUI.gameObject.SetActive(true);
        SetCursorEnabled(false);
    }

    private void FixedUpdate()
    {
    }

    #endregion

    private static void SetCursorEnabled(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
    }

    #region Game State Methods

    public void Pause()
    {
        Time.timeScale = 0f;
        State = GameState.Paused;
        pauseMenu.SetActive(true);

        EffectsController.Instance.SetDepthOfField(true);
        SetCursorEnabled(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        State = GameState.InProgress;
        pauseMenu.SetActive(false);

        EffectsController.Instance.SetDepthOfField(false);
        SetCursorEnabled(false);
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);
        
        State = GameState.Paused;
        gameOverMenu.SetActive(true);

        EffectsController.Instance.SetDepthOfField(true);
        SetCursorEnabled(true);
    }

    #endregion

    public IEnumerator SlowDownEffect(float scale = 0.5f, float duration = 0.25f)
    {
        // Slow down
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        EffectsController.Instance.SetChromaticAberration(true);
        EffectsController.Instance.SetVignetteIntensity(EffectsController.DefaultVignetteIntensity + 0.1f);

        yield return new WaitForSeconds(duration);

        // Back to normal
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        EffectsController.Instance.SetChromaticAberration(false);
        EffectsController.Instance.SetVignetteIntensity(EffectsController.DefaultVignetteIntensity);
    }

    public void SendUIMessage(string message)
    {
        if (string.IsNullOrEmpty(message)) return;

        uiMessage.gameObject.SetActive(false);
        _uiMessageText.text = message;
        uiMessage.gameObject.SetActive(true);
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}