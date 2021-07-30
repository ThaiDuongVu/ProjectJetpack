using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    [SerializeField] private bool disableOnStartup;
    [SerializeField] private Selector selector;

    public bool IsActive { get; set; }
    public bool IsInteractable { get; set; } = true;

    [SerializeField] private Transform buttonsParent;
    public Button[] Buttons { get; set; }
    public Animator[] ButtonAnimators { get; set; }
    public int SelectedButtonIndex { get; set; }

    [SerializeField] private Button backButton;

    [SerializeField] private Image layer;

    private InputManager inputManager;

    /// <summary>
    /// Unity Event function.
    /// On current object enabled.
    /// </summary>
    private void OnEnable()
    {
        inputManager = new InputManager();

        // Handle game UI input
        inputManager.Game.Direction.performed += OnDirectionPerformed;
        inputManager.Game.Click.performed += OnClickPerformed;
        inputManager.Game.Back.performed += OnBackPerformed;

        inputManager.Enable();

        selector.Select(SelectedButtonIndex);
    }

    #region Input Methods

    /// <summary>
    /// On directional input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void OnDirectionPerformed(InputAction.CallbackContext context)
    {
        if (!IsInteractable) return;

        Vector2 direction = context.ReadValue<Vector2>();

        // Pick button to select
        if (direction.y > 0f)
        {
            if (SelectedButtonIndex > 0) SelectedButtonIndex--;
            else SelectedButtonIndex = Buttons.Length - 1;
        }
        else
        {
            if (SelectedButtonIndex < Buttons.Length - 1) SelectedButtonIndex++;
            else SelectedButtonIndex = 0;
        }

        // Select new button
        selector.Select(SelectedButtonIndex);
    }

    /// <summary>
    /// On click input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        if (!IsInteractable) return;

        selector.Click(Buttons[SelectedButtonIndex]);
    }

    /// <summary>
    /// On back input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void OnBackPerformed(InputAction.CallbackContext context)
    {
        if (!IsInteractable || !backButton) return;

        selector.Click(backButton);
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
        Buttons = buttonsParent.GetComponentsInChildren<Button>();
        ButtonAnimators = buttonsParent.GetComponentsInChildren<Animator>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        if (disableOnStartup) SetActive(false);

        selector.Select(SelectedButtonIndex);
    }

    /// <summary>
    /// Set current menu active/inactive.
    /// </summary>
    /// <param name="value">Value to set</param>
    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    /// <summary>
    /// Set current menu interactable/uninteractable.
    /// </summary>
    /// <param name="value">Value to set</param>
    public void SetInteractable(bool value)
    {
        layer.gameObject.SetActive(!value);
        selector.Animator.enabled = value;

        IsInteractable = value;
    }

    /// <summary>
    /// Navigate to the next menu.
    /// </summary>
    /// <param name="next">Menu to navigate to</param>
    public void NextMenu(Menu next)
    {
        SetInteractable(false);
        next.SetActive(true);
    }

    /// <summary>
    /// Navigate to the previous menu.
    /// </summary>
    /// <param name="previous">Menu to navigate to</param>
    public void PreviousMenu(Menu previous)
    {
        previous.SetInteractable(true);
        SetActive(false);
    }
}
