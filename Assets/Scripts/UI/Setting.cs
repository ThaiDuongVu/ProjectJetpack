using UnityEngine;
using TMPro;

public class Setting : MonoBehaviour
{
    public int[] toggles;
    private int toggleIndex;

    public int CurrentState { get; set; }

    public string propertyName;
    public TMP_Text propertyText;
    public string[] properties;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        toggleIndex = PlayerPrefs.GetInt(propertyName, 0);

        CurrentState = toggles[toggleIndex];
        propertyText.text = properties[toggleIndex];
    }

    /// <summary>
    /// Cycle between settings.
    /// </summary>
    public void Toggle()
    {
        if (toggleIndex < toggles.Length - 1) toggleIndex++;
        else toggleIndex = 0;

        CurrentState = toggles[toggleIndex];
        propertyText.text = properties[toggleIndex];

        PlayerPrefs.SetInt(propertyName, toggleIndex);

        SettingsController.Instance.Apply();
    }
}