using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class GamepadRumbler : MonoBehaviour
{
    // Use the singleton pattern to make the class globally accessible

    #region Singleton

    private static GamepadRumbler instance;

    public static GamepadRumbler Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GamepadRumbler>();
            return instance;
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

    #region Rumble Method

    /// <summary>
    /// Rumble gamepad.
    /// </summary>
    /// <param name="gamepadRumbleMode">Mode at which to rumble</param>
    public void Rumble(GamepadRumbleMode gamepadRumbleMode)
    {
        // If no gamepad connected or vibration disabled then return
        if (Gamepad.current == null || PlayerPrefs.GetInt("GamepadVibration", 0) == 1) return;

        StopAllCoroutines();
        switch (gamepadRumbleMode)
        {
            case GamepadRumbleMode.Micro:
                StartCoroutine(StartRumble(0.1f, 0.1f));
                break;

            case GamepadRumbleMode.Light:
                StartCoroutine(StartRumble(0.12f, 0.12f));
                break;

            case GamepadRumbleMode.Normal:
                StartCoroutine(StartRumble(0.15f, 0.15f));
                break;

            case GamepadRumbleMode.Hard:
                StartCoroutine(StartRumble(0.2f, 0.2f));
                break;

            default:
                return;
        }
    }

    #endregion
}