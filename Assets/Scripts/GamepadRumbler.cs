using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class GamepadRumbler : MonoBehaviour
{
    // Use the singleton pattern to make the class globally accessible

    #region Singleton

    private static GamepadRumbler gamepadRumblerInstance;

    public static GamepadRumbler Instance
    {
        get
        {
            if (gamepadRumblerInstance == null) gamepadRumblerInstance = FindObjectOfType<GamepadRumbler>();
            return gamepadRumblerInstance;
        }
    }

    #endregion

    /// <summary>
    /// Start rumbling gamepad.
    /// </summary>
    /// <param name="duration">Number of seconds to rumble</param>
    /// <param name="intensity">How hard to rumble</param>
    /// <returns></returns>
    private static IEnumerator StartRumble(float duration, float intensity)
    {
        Gamepad.current.SetMotorSpeeds(intensity, intensity);
        yield return new WaitForSeconds(duration);

        InputSystem.ResetHaptics();
    }

    #region Rumble Methods

    /// <summary>
    /// Rumble connected gamepad.
    /// </summary>
    /// <param name="gamepadRumbleMode">Mode at which to rumble</param>
    public void Rumble(GamepadRumbleMode gamepadRumbleMode)
    {
        // If no gamepad connected or vibration disabled then return
        if (Gamepad.current == null || PlayerPrefs.GetInt("GamepadVibration", 0) == 1) return;

        StopAllCoroutines();
        switch (gamepadRumbleMode)
        {
            case GamepadRumbleMode.Nano:
                StartCoroutine(StartRumble(0.01f, 0.01f));
                break;

            case GamepadRumbleMode.Micro:
                StartCoroutine(StartRumble(0.05f, 0.05f));
                break;

            case GamepadRumbleMode.Light:
                StartCoroutine(StartRumble(0.075f, 0.075f));
                break;

            case GamepadRumbleMode.Normal:
                StartCoroutine(StartRumble(0.1f, 0.1f));
                break;

            case GamepadRumbleMode.Hard:
                StartCoroutine(StartRumble(0.15f, 0.15f));
                break;

            default:
                return;
        }
    }

    /// <summary>
    /// Rumble connected gamepad.
    /// </summary>
    /// <param name="leftHaptic">How rough should the left motor vibrate</param>
    /// <param name="rightHaptic">How rough should the right motor vibrate</param>
    public void Rumble(float leftHaptic, float rightHaptic)
    {
        // If no gamepad connected or vibration disabled then return
        if (Gamepad.current == null || PlayerPrefs.GetInt("GamepadVibration", 0) == 1) return;

        StopAllCoroutines();
        StartCoroutine(StartRumble(leftHaptic, rightHaptic));
    }

    #endregion
}