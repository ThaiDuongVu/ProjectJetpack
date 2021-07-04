using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    private EventSystem eventSystem;
    private PointerEventData eventData;

    private const float InterpolationRatio = 0.4f;
    private const float YOffset = -25f;
    private Vector2 lerpPosition;

    public Menu Menu { get; set; }

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        eventSystem = EventSystem.current;
        eventData = new PointerEventData(eventSystem);
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {

    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, lerpPosition, InterpolationRatio);
    }

    /// <summary>
    /// Select a menu button.
    /// </summary>
    /// <param name="button">Button to select</param>
    public void Select(Button button)
    {
        lerpPosition = new Vector2(button.transform.position.x, button.transform.position.y + YOffset);

        for (int i = 0; i < Menu.Buttons.Length; i++)
        {
            if (Menu.Buttons[i].Equals(button)) 
            {
                Menu.SelectedButtonIndex = i;
                break;
            }
        }
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
