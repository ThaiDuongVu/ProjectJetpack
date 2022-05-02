using UnityEngine;
using TMPro;

public class InputPrompt : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private string mouseKeyboardPrompt;
    [SerializeField] private string gamepadPrompt;

    private void FixedUpdate()
    {
        text.text = InputTypeController.Instance.InputType == InputType.MouseKeyboard
            ? mouseKeyboardPrompt
            : gamepadPrompt;
    }
}