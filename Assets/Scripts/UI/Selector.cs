using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    private PointerEventData _eventData;

    public Menu Menu { get; set; }

    private RectTransform _rectTransform;
    private Vector2 _lerpPosition;
    private const float LerpInterpolationRatio = 0.2f;
    private readonly Vector2 _offset = new Vector2(-110f, 0f);

    public Animator Animator { get; private set; }
    private static readonly int IsSelectedAnimatorTrigger = Animator.StringToHash("isSelected");

    #region Unity Event

    private void Awake()
    {
        _eventData = new PointerEventData(EventSystem.current);

        _rectTransform = GetComponent<RectTransform>();
        _lerpPosition = _rectTransform.anchoredPosition;

        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _rectTransform.anchoredPosition =
            Vector2.Lerp(_rectTransform.anchoredPosition, _lerpPosition, LerpInterpolationRatio);
    }

    #endregion

    public void SelectButton(int buttonIndex)
    {
        // Update button index & selector position
        Menu.SelectedButtonIndex = buttonIndex;
        _lerpPosition = Menu.Buttons[buttonIndex].GetComponent<RectTransform>().anchoredPosition + _offset;

        // Update button animations accordingly
        foreach (var animator in Menu.ButtonAnimators) animator.SetBool(IsSelectedAnimatorTrigger, false);
        Menu.ButtonAnimators[buttonIndex].SetBool(IsSelectedAnimatorTrigger, true);
    }

    public void ClickButton(Button button)
    {
        button.OnPointerClick(_eventData);
    }
}