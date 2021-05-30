using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    private Player player;

    #region Resources

    private float currentHealth;
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            player.Hud.UpdateHealthDisplay(value);
        }
    }
    public float MaxHealth { get; set; } = 10f;

    private int collectedCoins;
    public int CollectedCoins
    {
        get => collectedCoins;
        set
        {
            collectedCoins = value;
            player.Hud.UpdateCoinDisplay(value);
        }
    }

    #endregion

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        player = GetComponent<Player>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        CurrentHealth = MaxHealth;
    }
}
