using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    // Use the singleton pattern to make the class globally accessible

    #region Singleton

    private static CameraShaker cameraShakerInstance;

    public static CameraShaker Instance
    {
        get
        {
            if (cameraShakerInstance == null) cameraShakerInstance = FindObjectOfType<CameraShaker>();
            return cameraShakerInstance;
        }
    }

    #endregion

    private float shakeDuration;
    private float shakeIntensity;
    private float shakeDecreaseFactor;

    private Vector3 originalPosition;

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        originalPosition = transform.position;
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        Randomize();
    }

    /// <summary>
    /// Randomize camera position by shake intensity if is shaking.
    /// </summary>
    private void Randomize()
    {
        // While shake duration is greater than 0
        if (shakeDuration > 0)
        {
            // Randomize position
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeIntensity;
            // Decrease shake duration
            shakeDuration -= Time.fixedDeltaTime * shakeDecreaseFactor * Time.timeScale;
        }
        // When shake duration reaches 0
        else
        {
            // Reset everything
            shakeDuration = 0f;
            transform.localPosition = originalPosition;
        }
    }

    #region Shake Methods

    /// <summary>
    /// Start shaking camera.
    /// </summary>
    /// <param name="cameraShakeMode">Mode at which to shake</param>
    public void Shake(CameraShakeMode cameraShakeMode)
    {
        // If screen shake disabled in menu then do nothing
        if (PlayerPrefs.GetInt("ScreenShake", 0) == 1) return;

        originalPosition = new Vector3(0f, 0f, -10f);

        switch (cameraShakeMode)
        {
            case CameraShakeMode.Nano:
                shakeDuration = shakeIntensity = 0.04f;
                GamepadRumbler.Instance.Rumble(GamepadRumbleMode.Nano);
                break;
                
            case CameraShakeMode.Micro:
                shakeDuration = shakeIntensity = 0.08f;
                GamepadRumbler.Instance.Rumble(GamepadRumbleMode.Micro);
                break;

            case CameraShakeMode.Light:
                shakeDuration = shakeIntensity = 0.12f;
                GamepadRumbler.Instance.Rumble(GamepadRumbleMode.Light);
                break;

            case CameraShakeMode.Normal:
                shakeDuration = shakeIntensity = 0.15f;
                GamepadRumbler.Instance.Rumble(GamepadRumbleMode.Normal);
                break;

            case CameraShakeMode.Hard:
                shakeDuration = shakeIntensity = 0.2f;
                GamepadRumbler.Instance.Rumble(GamepadRumbleMode.Hard);
                break;

            default:
                return;
        }

        shakeDecreaseFactor = 2f;
    }

    /// <summary>
    /// Start shaking camera.
    /// </summary>
    /// <param name="duration">How long to shake for</param>
    /// <param name="intensity">How rough to shake for</param>
    /// <param name="decreaseFactor">How long until shaking stops</param>
    public void Shake(float duration, float intensity, float decreaseFactor)
    {
        originalPosition = new Vector3(0f, 0f, -10f);

        shakeDuration = duration;
        shakeIntensity = intensity;

        shakeDecreaseFactor = decreaseFactor;
    }

    #endregion
}