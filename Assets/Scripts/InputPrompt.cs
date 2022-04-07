using UnityEngine;
using TMPro;

public class InputPrompt : MonoBehaviour
{
    [SerializeField] private string promptKeyboard;
    [SerializeField] private string promptGamepad;

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private TMP_Text text;
    private Camera mainCamera;

    #region Unity Event

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        text.transform.position = mainCamera.WorldToScreenPoint(transform.position);
        text.text = InputTypeController.Instance.InputType == InputType.Gamepad ? promptGamepad : promptKeyboard;
    }

    #endregion
}
