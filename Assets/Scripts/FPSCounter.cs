using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private int fpsLowerBound = 50;
    [SerializeField] private float updateDelay = 1f;
    [SerializeField] private float updateInterval = 0.5f;

    private TMP_Text _text;

    #region Unity Event

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        InvokeRepeating(nameof(UpdateFPS), updateDelay, updateInterval);
    }

    #endregion

    private void UpdateFPS()
    {
        var fps = (int)(1f / Time.unscaledDeltaTime);
        _text.text = fps.ToString();

        if (fps <= fpsLowerBound) Debug.Break();
    }
}
