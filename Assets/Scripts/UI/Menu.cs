using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    [SerializeField] private bool disableOnStartup;
    [SerializeField] private GameObject elements;
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private Selector selector;

    public int SelectedButtonIndex { get; set; }
    public Button[] Buttons { get; set; }

    private InputManager inputManager;

    /// <summary>
    /// Unity Event function.
    /// Initialize input handler on object enabled.
    /// </summary>
    private void OnEnable()
    {
        inputManager = new InputManager();

        // Handle menu inputs
        inputManager.Game.Direction.performed += DirectionOnPerformed;
        inputManager.Game.Click.performed += ClickOnPerformed;

        inputManager.Enable();

        selector.Select(Buttons[SelectedButtonIndex]);
    }

    #region Input Methods

    /// <summary>
    /// On direction input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void DirectionOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);

        if (context.ReadValue<Vector2>().y > 0f)
        {
            if (SelectedButtonIndex > 0) SelectedButtonIndex--;
            else SelectedButtonIndex = Buttons.Length - 1;
        }
        else
        {
            if (SelectedButtonIndex < Buttons.Length - 1) SelectedButtonIndex++;
            else SelectedButtonIndex = 0;
        }
        selector.Select(Buttons[SelectedButtonIndex]);
    }

    /// <summary>
    /// On click input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void ClickOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        selector.Click(Buttons[SelectedButtonIndex]);
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
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        Buttons = buttonsParent.GetComponentsInChildren<Button>();
        selector.Menu = this;
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        if (disableOnStartup) SetActive(false);
    }

    /// <summary>
    /// Set whether current menu is active.
    /// </summary>
    /// <param name="value">Value to set</param>
    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}
