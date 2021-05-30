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

    public Settings fullScreen;
    public Settings resolution;

    public Settings effects;
    public Volume volume;

    public Settings font;
    public TMP_FontAsset[] fonts;

    public new Settings audio;

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
        Screen.SetResolution(resolution.currentState, resolution.currentState / 16 * 9,
            (FullScreenMode) fullScreen.currentState);

        // Apply effects setting
        volume.enabled = effects.currentState == 1;

        // Apply font setting
        foreach (Object o in Resources.FindObjectsOfTypeAll(typeof(TMP_Text)))
        {
            TMP_Text text = (TMP_Text) o;
            if (text.CompareTag("Title")) continue;
            text.font = fonts[font.currentState];
        }

        // Apply audio setting
        foreach (Object o in Resources.FindObjectsOfTypeAll(typeof(AudioSource)))
        {
            AudioSource audioSource = (AudioSource) o;
            audioSource.enabled = false;
        }
    }
}