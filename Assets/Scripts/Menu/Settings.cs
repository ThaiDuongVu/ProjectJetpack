using UnityEngine;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] private bool isEnabled;

    public int[] toggles;
    private int toggleIndex;

    [HideInInspector] public int currentState;

    public string propertyName;
    public TMP_Text propertyText;
    public string[] properties;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        if (!isEnabled) return;

        toggleIndex = PlayerPrefs.GetInt(propertyName, 0);

        currentState = toggles[toggleIndex];
        propertyText.text = properties[toggleIndex];
    }

    /// <summary>
    /// Cycle between settings.
    /// </summary>
    public void Toggle()
    {
        if (toggleIndex < toggles.Length - 1)
            toggleIndex++;
        else
            toggleIndex = 0;

        currentState = toggles[toggleIndex];
        propertyText.text = properties[toggleIndex];

        PlayerPrefs.SetInt(propertyName, toggleIndex);

        SettingsController.Instance.Apply();
    }
}