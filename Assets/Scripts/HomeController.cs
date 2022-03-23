using UnityEngine;

public class HomeController : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private Canvas mainUI;

    [Header("UI Message")]
    [SerializeField] private GameObject uiMessage;

    #region Unity Event

    private void Start()
    {
        EffectsController.Instance.SetDepthOfField(false);
        EffectsController.Instance.SetChromaticAberration(false);
        EffectsController.Instance.SetVignetteIntensity(EffectsController.DefaultVignetteIntensity);

        uiMessage.gameObject.SetActive(false);
        mainUI.gameObject.SetActive(true);
        SetCursorEnabled(true);
    }

    #endregion

    private static void SetCursorEnabled(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
    }
}
