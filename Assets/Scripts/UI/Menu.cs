using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public bool IsActive { get; private set; }
    [SerializeField] private bool disableOnStartup;
    [SerializeField] private Selector selector;

    public bool IsInteractable { get; private set; } = true;

    [SerializeField] private Transform buttonsParent;
    public Button[] Buttons { get; private set; }
    public Animator[] ButtonAnimators { get; private set; }
    public int SelectedButtonIndex { get; set; }

    [SerializeField] private Button backButton;

    [SerializeField] private Image layer;

    private InputManager _inputManager;

    #region Input Methods

    private void DirectionOnStarted(InputAction.CallbackContext context)
    {
        if (!IsInteractable) return;
        InputTypeController.Instance.CheckInputType(context);

        Vector2 direction = context.ReadValue<Vector2>();

        // Decide which button to select
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
        selector.SelectButton(SelectedButtonIndex);
    }

    private void ClickOnStarted(InputAction.CallbackContext context)
    {
        if (!IsInteractable) return;
        InputTypeController.Instance.CheckInputType(context);

        selector.ClickButton(Buttons[SelectedButtonIndex]);
    }

    private void BackOnStarted(InputAction.CallbackContext context)
    {
        if (!IsInteractable || !backButton) return;
        InputTypeController.Instance.CheckInputType(context);

        selector.ClickButton(backButton);
    }

    #endregion

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle game UI input
        _inputManager.Game.Direction.started += DirectionOnStarted;
        _inputManager.Game.Click.started += ClickOnStarted;
        _inputManager.Game.Back.started += BackOnStarted;

        _inputManager.Enable();

        selector.SelectButton(SelectedButtonIndex);
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    private void Awake()
    {
        Buttons = buttonsParent.GetComponentsInChildren<Button>();
        ButtonAnimators = buttonsParent.GetComponentsInChildren<Animator>();

        selector.Menu = this;
    }

    private void Start()
    {
        if (disableOnStartup) SetActive(false);

        selector.SelectButton(SelectedButtonIndex);
        for (var i = 0; i < Buttons.Length; i++) InitButton(i);
    }

    #endregion

    private void InitButton(int index)
    {
        var eventTrigger = Buttons[index].GetComponent<EventTrigger>();
        var entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };

        entry.callback.AddListener((_) => { selector.SelectButton(index); });
        eventTrigger.triggers.Add(entry);
    }

    #region Menu Control

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
        IsActive = value;
    }

    public void SetInteractable(bool value)
    {
        layer.gameObject.SetActive(!value);
        selector.Animator.enabled = value;

        IsInteractable = value;
    }

    #endregion

    #region Menu Navigation

    public void NextMenu(Menu next)
    {
        SetInteractable(false);
        next.SetActive(true);
    }

    public void PreviousMenu(Menu previous)
    {
        previous.SetInteractable(true);
        SetActive(false);
    }

    #endregion
}