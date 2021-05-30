using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    private EventSystem eventSystem;
    private PointerEventData eventData;

    private Button currentSelected;

    private RectTransform rectTransform;
    private Vector2 lerpPosition;
    private const float InterpolationRatio = 0.25f;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        eventSystem = EventSystem.current;
        eventData = new PointerEventData(eventSystem);

        rectTransform = transform.GetComponent<RectTransform>();
        lerpPosition = rectTransform.anchoredPosition;
    }

    /// <summary>
    /// Unity Event function.
    /// Update once per frame.
    /// </summary>
    private void Update()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, lerpPosition, InterpolationRatio);
    }

    /// <summary>
    /// Select a button.
    /// </summary>
    /// <param name="button">Button to select</param>
    public void Select(Button button)
    {
        currentSelected = button;
        button.OnPointerEnter(eventData);

        lerpPosition.y = button.GetComponent<RectTransform>().anchoredPosition.y;
    }

    /// <summary>
    /// Deselect a button.
    /// </summary>
    /// <param name="button">Button to deselect</param>
    public void Deselect(Button button)
    {
        currentSelected = null;
        button.OnPointerExit(eventData);
    }

    /// <summary>
    /// Click current selected button.
    /// </summary>
    public void Click()
    {
        currentSelected.OnPointerClick(eventData);
    }
}