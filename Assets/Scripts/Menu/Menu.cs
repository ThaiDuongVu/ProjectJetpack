using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    [SerializeField] private bool disableOnStartup;
    [SerializeField] private Selector selector;
    [SerializeField] private Button[] buttons;
    private int currentButtonIndex;

    [SerializeField] private Image background;

    private bool isInteractable;

    private InputManager inputManager;

    /// <summary>
    /// Unity Event function.
    /// Initialize input handler on object enabled.
    /// </summary>
    private void OnEnable()
    {
        inputManager = new InputManager();

        // Handle menu input
        inputManager.Game.Direction.performed += DirectionOnPerformed;
        inputManager.Game.Click.started += ClickOnStarted;

        inputManager.Enable();

        // Select first button
        selector.Select(buttons[0]);

        // Set menu active to true
        isInteractable = true;
    }

    #region Input Methods

    /// <summary>
    /// On direction input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void DirectionOnPerformed(InputAction.CallbackContext context)
    {
        if (!isInteractable) return;

        Vector2 direction = context.ReadValue<Vector2>();

        // Up direction input
        if (direction.y > 0)
            currentButtonIndex = currentButtonIndex == 0 ? buttons.Length - 1 : currentButtonIndex - 1;
        // Down direction input
        else if (direction.y < 0)
            currentButtonIndex = currentButtonIndex == buttons.Length - 1 ? 0 : currentButtonIndex + 1;

        SelectButton(currentButtonIndex);
    }

    /// <summary>
    /// On click input started.
    /// </summary>
    /// <param name="context">Input context</param>
    private void ClickOnStarted(InputAction.CallbackContext context)
    {
        if (!isInteractable) return;

        // Click current button
        selector.Click();
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
        if (disableOnStartup) gameObject.SetActive(false);

        SetInteractable(true);
    }

    /// <summary>
    /// Select a button.
    /// </summary>
    /// <param name="buttonIndex">Button to select</param>
    public void SelectButton(int buttonIndex)
    {
        // Deselect all buttons first
        foreach (Button button1 in buttons)
            selector.Deselect(button1);

        // Select button
        currentButtonIndex = buttonIndex;
        selector.Select(buttons[buttonIndex]);
    }

    /// <summary>
    /// Set whether menu is interactable or not.
    /// </summary>
    /// <param name="value">Interactivity of menu</param>
    public void SetInteractable(bool value)
    {
        isInteractable = value;
        background.gameObject.SetActive(!value);

        selector.gameObject.SetActive(value);
    }

    /// <summary>
    /// Set whether menu is visible or not.
    /// </summary>
    /// <param name="value">Visibility of menu</param>
    public void SetEnabled(bool value)
    {
        gameObject.SetActive(value);
    }
}