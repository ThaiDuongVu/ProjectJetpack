using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text coinText;

    // <summary>
    /// Update player health bar to reflect current health.
    /// </summary>
    /// <param name="newHealth">Health to display</param>
    public void UpdateHealthDisplay(float newHealth)
    {
        healthBar.fillAmount = newHealth / Player.Instance.Resources.MaxHealth;
    }

    /// <summary>
    /// Update player coin text to reflect collected coins.
    /// </summary>
    /// <param name="newCollected">Coins to display</param>
    public void UpdateCoinDisplay(int newCollected)
    {
        coinText.text = newCollected.ToString();
    }
}
