using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectsController : MonoBehaviour
{
    #region Singleton

    private static EffectsController _effectsControllerInstance;

    public static EffectsController Instance
    {
        get
        {
            if (_effectsControllerInstance == null) _effectsControllerInstance = FindObjectOfType<EffectsController>();
            return _effectsControllerInstance;
        }
    }

    #endregion

    [SerializeField] private VolumeProfile volumeProfile;
    private DepthOfField _depthOfField;
    private ChromaticAberration _chromaticAberration;
    private Vignette _vignette;

    public const float DefaultVignetteIntensity = 0.5f;
    public const float DefaultChromaticAberrationIntensity = 0.4f;

    #region Unity Event

    private void Awake()
    {
        volumeProfile.TryGet(out _depthOfField);
        volumeProfile.TryGet(out _chromaticAberration);
        volumeProfile.TryGet(out _vignette);

        SetChromaticAberration(false);
        SetChromaticAberrationIntensity(DefaultChromaticAberrationIntensity);
        SetVignetteIntensity(DefaultVignetteIntensity);
    }

    #endregion

    public void SetDepthOfField(bool value)
    {
        _depthOfField.active = value;
    }

    public void SetChromaticAberration(bool value)
    {
        _chromaticAberration.active = value;
    }

    private void SetChromaticAberrationIntensity(float value)
    {
        _chromaticAberration.intensity.value = value;
    }

    public void SetVignetteIntensity(float value)
    {
        _vignette.intensity.value = value;
    }
}