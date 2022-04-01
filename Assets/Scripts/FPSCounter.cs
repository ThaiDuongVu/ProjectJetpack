using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private int fpsLowerBound = 50;
    [SerializeField] private float updateDelay = 1f;
    [SerializeField] private float updateInterval = 0.5f;

    private TMP_Text text;

    #region Unity Event

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        InvokeRepeating(nameof(UpdateFPS), updateInterval, updateInterval);
    }

    #endregion

    private void UpdateFPS()
    {
        var fps = (int)(1f / Time.unscaledDeltaTime);
        text.text = fps.ToString();

        if (fps < fpsLowerBound) Debug.Break();
    }
}
