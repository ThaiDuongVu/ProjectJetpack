using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    [SerializeField] private bool disableOnStartup;
    [SerializeField] private Selector selector;

    public bool IsInteractable { get; private set; } = true;

    [SerializeField] private Transform buttonsParent;
    public Button[] Buttons { get; private set; }
    public Animator[] ButtonAnimators { get; private set; }
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
        inputManager.Game.Direction.performed += DirectionOnPerformed;
        inputManager.Game.Click.performed += ClickOnPerformed;
        inputManager.Game.Back.performed += BackOnPerformed;

        inputManager.Enable();

        selector.Select(SelectedButtonIndex);
    }

    #region Input Methods

    /// <summary>
    /// On directional input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void DirectionOnPerformed(InputAction.CallbackContext context)
    {
        if (!IsInteractable) return;
        InputTypeController.Instance.CheckInputType(context);

        Vector2 direction = context.ReadValue<Vector2>();

        // Pick button to select
        if (direction.y > 0f)
        {
            if (SelectedButtonIndex > 0) SelectedButtonIndex--;
            else SelectedButtonIndex = Buttons.Length - 1;
        }
        else if (direction.y < 0f)
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
    private void ClickOnPerformed(InputAction.CallbackContext context)
    {
        if (!IsInteractable) return;
        InputTypeController.Instance.CheckInputType(context);

        selector.Click(Buttons[SelectedButtonIndex]);
    }

    /// <summary>
    /// On back input performed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void BackOnPerformed(InputAction.CallbackContext context)
    {
        if (!IsInteractable || !backButton) return;
        InputTypeController.Instance.CheckInputType(context);

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

        selector.Menu = this;
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        if (disableOnStartup) SetActive(false);

        selector.Select(SelectedButtonIndex);
        for (var i = 0; i < Buttons.Length; i++) InitButton(i);
    }

    /// <summary>
    /// Add event trigger to handle pointer enter event on button.
    /// </summary>
    /// <param name="index">Index of button</param>
    private void InitButton(int index)
    {
        var eventTrigger = Buttons[index].GetComponent<EventTrigger>();
        var entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        
        entry.callback.AddListener((_) => { selector.Select(index); });
        eventTrigger.triggers.Add(entry);
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
    /// Set current menu interactable/non-interactable.
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
