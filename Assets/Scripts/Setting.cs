using UnityEngine;
using TMPro;

public class Setting : MonoBehaviour
{
    [SerializeField] private int[] toggles;
    private int _toggleIndex;

    public int CurrentState { get; private set; }

    [SerializeField] private string propertyName;
    [SerializeField] private TMP_Text propertyText;
    [SerializeField] private string[] properties;

    #region Unity Event

    private void Awake()
    {
        _toggleIndex = PlayerPrefs.GetInt(propertyName, 0);

        CurrentState = toggles[_toggleIndex];
        propertyText.text = properties[_toggleIndex];
    }

    #endregion

    public void Toggle()
    {
        // Cycle between settings
        if (_toggleIndex < toggles.Length - 1) _toggleIndex++;
        else _toggleIndex = 0;

        // Apply setting
        CurrentState = toggles[_toggleIndex];
        propertyText.text = properties[_toggleIndex];

        PlayerPrefs.SetInt(propertyName, _toggleIndex);
        SettingsController.Instance.Apply();
    }
}