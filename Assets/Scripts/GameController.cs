using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameController : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible

    #region Singleton

    private static GameController gameControllerInstance;

    public static GameController Instance
    {
        get
        {
            if (gameControllerInstance == null) gameControllerInstance = FindObjectOfType<GameController>();
            return gameControllerInstance;
        }
    }

    #endregion

    public GameState State { get; private set; } = GameState.Started;

    [SerializeField] private Menu pauseMenu;
    [SerializeField] private Menu gameOverMenu;

    [SerializeField] private GameObject uiMessage;
    private TMP_Text uiMessageText;

    private InputManager inputManager;

    /// <summary>
    /// Unity Event function.
    /// On current object enabled.
    /// </summary>
    private void OnEnable()
    {
        inputManager = new InputManager();

        // Handle game pause input
        inputManager.Game.Escape.performed += OnEscapePerformed;

        inputManager.Enable();
    }

    #region Input Methods

    /// <summary>
    /// On escape input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void OnEscapePerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);

        if (State == GameState.Started) Pause();
        else if (State == GameState.Paused) Resume();
    }

    #endregion

    /// <summary>
    /// Unity Event function.
    /// On current object disabled.
    /// </summary>
    private void OnDisable()
    {
        inputManager.Disable();
    }

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        uiMessageText = uiMessage.GetComponentInChildren<TMP_Text>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        EffectsController.Instance.SetDepthOfField(false);
        EffectsController.Instance.SetChromaticAberration(false);
        EffectsController.Instance.SetVignetteIntensity(EffectsController.DefaultVignetteIntensity);

        SetCursorEnabled(false);
        uiMessage.gameObject.SetActive(false);
    }

    /// <summary>
    /// Set whether cursor is enabled.
    /// </summary>
    /// <param name="value">Value to set</param>
    public void SetCursorEnabled(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }

    /// <summary>
    /// Pause current game.
    /// </summary>
    public void Pause()
    {
        State = GameState.Paused;

        Time.timeScale = 0f;
        EffectsController.Instance.SetDepthOfField(true);
        SetCursorEnabled(true);

        pauseMenu.SetActive(true);
    }

    /// <summary>
    /// Resume current game.
    /// </summary>
    public void Resume()
    {
        if (!pauseMenu.IsInteractable) return;
        State = GameState.Started;

        Time.timeScale = 1f;
        EffectsController.Instance.SetDepthOfField(false);
        SetCursorEnabled(false);

        pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Game over.
    /// </summary>
    public void GameOver()
    {
        State = GameState.Over;

        EffectsController.Instance.SetDepthOfField(true);
        SetCursorEnabled(true);

        gameOverMenu.SetActive(true);
    }

    /// <summary>
    /// Slow the game down for a slight amount of time.
    /// </summary>
    /// <param name="scale">Slowed time scale</param>
    /// <param name="duration">Duration to slow for</param>
    public IEnumerator SlowDownEffect(float scale = 0.5f, float duration = 0.4f)
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

    /// <summary>
    /// Send a string message to the player through the game's UI.
    /// </summary>
    /// <param name="message">Message to send</param>
    public void SendUIMessage(string message)
    {
        uiMessage.gameObject.SetActive(false);
        uiMessageText.text = message;
        uiMessage.gameObject.SetActive(true);
    }
}
