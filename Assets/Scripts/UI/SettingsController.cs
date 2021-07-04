using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class SettingsController : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible

    #region Singleton

    private static SettingsController instance;

    public static SettingsController Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<SettingsController>();
            return instance;
        }
    }

    #endregion

    public Setting fullScreen;
    public Setting resolution;
    public Setting quality;

    public Setting font;
    public TMP_FontAsset[] fonts;

    public new Setting audio;

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
        // Set target framerate to 60fps
        Application.targetFrameRate = 60;

        // Apply resolution & full screen settings
        Screen.SetResolution(resolution.CurrentState, resolution.CurrentState / 16 * 9,
            (FullScreenMode) fullScreen.CurrentState);

        // Apply quality settings
        QualitySettings.SetQualityLevel(quality.CurrentState);

        // Apply font setting
        foreach (Object o in Resources.FindObjectsOfTypeAll(typeof(TMP_Text)))
        {
            TMP_Text text = (TMP_Text) o;
            if (text.CompareTag("Title")) continue;
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