using UnityEngine;
using TMPro;

public class HomeController : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible

    #region Singleton

    private static HomeController instance;

    public static HomeController Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<HomeController>();
            return instance;
        }
    }

    #endregion

    [SerializeField] private TMP_Text promptText;

    /// <summary>
    /// Set prompt text message content.
    /// </summary>
    /// <param name="message">Message content to set</param>
    public void SetPromptText(string message)
    {
        promptText.text = message;
    }
}
