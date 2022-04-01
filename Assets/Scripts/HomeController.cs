using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private Canvas mainUI;

    [Header("UI Message")]
    [SerializeField] private GameObject uiMessage;

    private InputManager _inputManager;

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        _inputManager.Game.Test.performed += (InputAction.CallbackContext context) => { SceneManager.LoadScene("Playground"); };

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

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