using UnityEngine;
using TMPro;

public class Setting : MonoBehaviour
{
    [SerializeField] private int[] toggles;
    private int toggleIndex;

    public int CurrentState { get; private set; }

    [SerializeField] private string propertyName;
    [SerializeField] private TMP_Text propertyText;
    [SerializeField] private string[] properties;

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