using UnityEngine;
using TMPro;

public class SettingsController : MonoBehaviour
{
    #region Singleton

    private static SettingsController _settingsControllerInstance;

    public static SettingsController Instance
    {
        get
        {
            if (_settingsControllerInstance == null)
                _settingsControllerInstance = FindObjectOfType<SettingsController>();
            return _settingsControllerInstance;
        }
    }

    #endregion

    [SerializeField] private Setting fullScreen;
    [SerializeField] private Setting resolution;
    [SerializeField] private Setting quality;

    [SerializeField] private Setting font;
    [SerializeField] private TMP_FontAsset[] fonts;

    #region Unity Event

    private void Start()
    {
        Apply();
    }

    #endregion

    public void Apply()
    {
        Application.targetFrameRate = 60;
#if UNITY_STANDALONE
        // Apply resolution & full screen settings
        Screen.SetResolution(resolution.CurrentState, resolution.CurrentState / 16 * 9,
            (FullScreenMode) fullScreen.CurrentState);
#endif

        // Apply quality settings
        QualitySettings.SetQualityLevel(quality.CurrentState);

        // Apply font setting
        foreach (var obj in Resources.FindObjectsOfTypeAll(typeof(TMP_Text)))
        {
            var text = (TMP_Text) obj;
            if (text.CompareTag("IgnoreFont")) continue;
            text.font = fonts[font.CurrentState];
        }

        // // Apply audio setting
        // foreach (Object o in Resources.FindObjectsOfTypeAll(typeof(AudioSource)))
        // {
        //     AudioSource audioSource = (AudioSource) o;
        //     audioSource.enabled = false;
        // }
    }
}