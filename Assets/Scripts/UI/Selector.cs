using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    private EventSystem eventSystem;
    private PointerEventData eventData;

    [SerializeField] private Menu menu;

    private RectTransform rectTransform;
    private Vector2 lerpPosition;
    private const float LerpInterpolationRatio = 0.2f;
    private Vector2 offset = new Vector2(-100f, 0f);

    public Animator Animator { get; set; }

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        eventSystem = EventSystem.current;
        eventData = new PointerEventData(eventSystem);

        rectTransform = GetComponent<RectTransform>();
        lerpPosition = rectTransform.anchoredPosition;

        Animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Unity Event function.
    /// Update once per frame.
    /// </summary>
    private void Update()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, lerpPosition, LerpInterpolationRatio);
    }

    /// <summary>
    /// Select a menu button.
    /// </summary>
    /// <param name="button">Button to select</param>
    public void Select(int buttonIndex)
    {
        // Update button index & selector position
        menu.SelectedButtonIndex = buttonIndex;
        lerpPosition = menu.Buttons[buttonIndex].GetComponent<RectTransform>().anchoredPosition + offset;

        // Update button animations accordingly
        foreach (var animator in menu.ButtonAnimators) animator.SetBool("isSelected", false);
        menu.ButtonAnimators[buttonIndex].SetBool("isSelected", true);
    }

    /// <summary>
    /// Click a menu button.
    /// </summary>
    /// <param name="button">Button to click</param>
    public void Click(Button button)
    {
        button.OnPointerClick(eventData);
    }
}
