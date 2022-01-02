using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerResources : MonoBehaviour
{
    public Player Player { get; private set; }

    [Header("Token")]
    [SerializeField] private TMP_Text tokenText;
    private int token;
    public int Token
    {
        get => token;
        set
        {
            token = value;
            tokenText.text = value.ToString();
        }
    }

    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private Image healthDisplay;
    private float currentHealth;
    public float Health
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            healthDisplay.transform.localScale = new Vector2(value / maxHealth, 1f);

            if (value <= 0f) Player.Die();
        }
    }

    [Header("Fuel")]
    public float maxFuel;
    [SerializeField] private Image fuelDisplay;
    private float currentFuel;
    public float Fuel
    {
        get => currentFuel;
        set
        {
            currentFuel = value;
            currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
            fuelDisplay.transform.localScale = new Vector2(value / maxFuel, 1f);
        }
    }
    [SerializeField] private float fuelRefilRate = 10f;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        Player = GetComponent<Player>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        Token = 0;
        Health = maxHealth;
        Fuel = maxFuel;
    }
    
    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        if (Player.Movement.IsGrounded && Fuel < maxFuel) Fuel += fuelRefilRate * Time.fixedDeltaTime;
    }
}
