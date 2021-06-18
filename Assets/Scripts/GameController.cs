using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] private Menu[] otherMenus;

    private InputManager inputManager;

    /// <summary>
    /// Unity Event function.
    /// Initialize input handler on object enabled.
    /// </summary>
    private void OnEnable()
    {
        inputManager = new InputManager();

        // Handle various inputs
        inputManager.Game.Escape.performed += EscapeOnPerformed;
        inputManager.Game.Test.performed += TestOnPerformed;

        inputManager.Enable();
    }

    #region Input Methods

    /// <summary>
    /// On escape input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void EscapeOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (State == GameState.Started)
            Pause();
        else if (State == GameState.Paused)
            Resume();
    }

    /// <summary>
    /// On test input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void TestOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
    }

    #endregion

    /// <summary>
    /// Unity Event function.
    /// Disable input handling on object disabled.
    /// </summary>
    private void OnDisable()
    {
        inputManager.Disable();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        DisableCursor();
    }

    /// <summary>
    /// Disable current mouse cursor.
    /// </summary>
    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Enable current mouse cursor.
    /// </summary>
    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Pause current game.
    /// </summary>
    public void Pause()
    {
        // Update game state
        State = GameState.Paused;

        // Enable depth of field effect
        EffectsController.Instance.SetDepthOfField(true);
        // Enable pause menu
        pauseMenu.SetEnabled(true);
        // Enable mouse cursor
        EnableCursor();

        // Freeze game
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Resume current game.
    /// </summary>
    public void Resume()
    {
        // Update game state
        State = GameState.Started;

        // Disable depth of field effect
        EffectsController.Instance.SetDepthOfField(false);

        // Disable menus
        pauseMenu.SetEnabled(false);
        pauseMenu.SetInteractable(true);
        foreach (Menu menu in otherMenus)
        {
            menu.SetEnabled(false);
            menu.SetInteractable(true);
        }
        // Disable mouse cursor
        DisableCursor();

        // Unfreeze game
        Time.timeScale = 1f;
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