using UnityEngine;
using TMPro;

public class SettingsController : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible

    #region Singleton

    private static SettingsController settingsControllerInstance;

    public static SettingsController Instance
    {
        get
        {
            if (settingsControllerInstance == null) settingsControllerInstance = FindObjectOfType<SettingsController>();
            return settingsControllerInstance;
        }
    }

    #endregion

    [SerializeField] private Setting fullScreen;
    [SerializeField] private Setting resolution;
    [SerializeField] private Setting quality;

    [SerializeField] private Setting font;
    [SerializeField] private TMP_FontAsset[] fonts;

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        Apply();
    }

    /// <summary>
    /// Apply current settings.
    /// </summary>
    public void Apply()
    {
        Application.targetFrameRate = 60;

        // Apply resolution & full screen settings
        Screen.SetResolution(resolution.CurrentState, resolution.CurrentState / 16 * 9,
            (FullScreenMode)fullScreen.CurrentState);

        // Apply quality settings
        QualitySettings.SetQualityLevel(quality.CurrentState);

        // Apply font setting
        foreach (var obj in Resources.FindObjectsOfTypeAll(typeof(TMP_Text)))
        {
            var text = (TMP_Text)obj;
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