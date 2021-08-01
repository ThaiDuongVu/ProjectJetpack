using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible

    #region Singleton

    private static GameController instance;

    public static GameController Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameController>();
            return instance;
        }
    }

    #endregion

    public GameState State { get; set; } = GameState.Started;

    [SerializeField] private Menu pauseMenu;
    private Menu[] menus;

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
        menus = FindObjectsOfType<Menu>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        SetCursorEnabled(false);
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {

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
        if (HomeController.Instance) HomeController.Instance.SetPromptText("");

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
        if (HomeController.Instance) HomeController.Instance.SetPromptText("Press Escape to start");

        pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Slow the game down for a slight amount of time.
    /// </summary>
    /// <param name="scale">Slowed time scale</param>
    /// <param name="duration">Duration to slow for</param>
    public IEnumerator SlowDownEffect(float scale = 0.6f, float duration = 0.4f)
    {
        // Slow down
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        EffectsController.Instance.SetChromaticAberration(true);

        yield return new WaitForSeconds(duration);

        // Back to normal
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        EffectsController.Instance.SetChromaticAberration(false);
    }
}
