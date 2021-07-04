using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectsController : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible

    #region Singleton

    private static EffectsController instance;

    public static EffectsController Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<EffectsController>();
            return instance;
        }
    }

    #endregion

    public VolumeProfile volumeProfile;
    private DepthOfField depthOfField;
    private MotionBlur motionBlur;
    private ChromaticAberration chromaticAberration;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        volumeProfile.TryGet(out depthOfField);
        volumeProfile.TryGet(out motionBlur);
        volumeProfile.TryGet(out chromaticAberration);
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        SetDepthOfField(false);
        SetChromaticAberration(false);
        // SetMotionBlur(false);
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
    /// Enable/disable motion blur effect.
    /// </summary>
    /// <param name="value">Enable state</param>
    public void SetMotionBlur(bool value)
    {
        motionBlur.active = value;
    }

    /// <summary>
    /// Enable/disable chromatic aberration effect.
    /// </summary>
    /// <param name="value">Enable state</param>
    public void SetChromaticAberration(bool value)
    {
        chromaticAberration.active = value;
    }
}
