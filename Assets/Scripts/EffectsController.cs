using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectsController : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible

    #region Singleton

    private static EffectsController effectsControllerInstance;

    public static EffectsController Instance
    {
        get
        {
            if (effectsControllerInstance == null) effectsControllerInstance = FindObjectOfType<EffectsController>();
            return effectsControllerInstance;
        }
    }

    #endregion

    [SerializeField] private VolumeProfile volumeProfile;
    private DepthOfField depthOfField;
    private ChromaticAberration chromaticAberration;
    private Vignette vignette;

    public const float DefaultVignetteIntensity = 0.3f;
    public const float DefaultChromaticAberrationIntensity = 0.2f;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        volumeProfile.TryGet(out depthOfField);
        volumeProfile.TryGet(out chromaticAberration);
        volumeProfile.TryGet(out vignette);

        SetChromaticAberration(false);
        SetChromaticAberrationIntensity(DefaultChromaticAberrationIntensity);
        SetVignetteIntensity(DefaultVignetteIntensity);
    }

    /// <summary>
    /// Enable/disable depth of field effect.
    /// </summary>
    /// <param name="value">Enable state</param>
    public void SetDepthOfField(bool value)
    {
        depthOfField.active = value;
    }

    /// <summary>
    /// Enable/disable chromatic aberration effect.
    /// </summary>
    /// <param name="value">Enable state</param>
    public void SetChromaticAberration(bool value)
    {
        chromaticAberration.active = value;
    }

    /// <summary>
    /// Set chromatic aberration effect intensity.
    /// </summary>
    /// <param name="value">Value to set</param>
    private void SetChromaticAberrationIntensity(float value)
    {
        chromaticAberration.intensity.value = value;
    }

    /// <summary>
    /// Set vignette effect intensity.
    /// </summary>
    /// <param name="value">Value to set</param>
    public void SetVignetteIntensity(float value)
    {
        vignette.intensity.value = value;
    }
}
