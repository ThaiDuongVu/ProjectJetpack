using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerResources : CharacterResources
{
    private Player _player;

    [SerializeField] private Transform healthDisplay;
    public int lowHealthThreshold = 3;
    private Animator _healthDisplayAnimator;
    private static readonly int IsLowHealthAnimationTrigger = Animator.StringToHash("isLowHealth");
    private Image[] _healthIcons;
    public const string HealthTempKey = "TempHealth";
    public override int Health
    {
        get => base.Health;
        set
        {
            base.Health = value;
            _healthDisplayAnimator.SetBool(IsLowHealthAnimationTrigger, value <= lowHealthThreshold);
            UpdateHealthBar();

            if (value <= 0) _player.Die();
        }
    }

    [Header("Token")]
    [SerializeField] private TMP_Text tokenText;
    private const string TokenTempKey = "TempToken";
    private int _token;
    public int Token
    {
        get => _token;
        set
        {
            _token = value;
            tokenText.text = value.ToString();
        }
    }

    [Header("Fuel")]
    [SerializeField] private Image fuelBar;
    public float maxFuel = 1f;
    private const string FuelTempKey = "TempFuel";
    private float _fuel;
    public float Fuel
    {
        get => _fuel;
        set
        {
            _fuel = Mathf.Clamp(value, 0f, maxFuel);
            UpdateFuelBar();
        }
    }

    public const string LevelIndexKey = "LevelIndex";

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _player = GetComponent<Player>();

        _healthIcons = healthDisplay.GetComponentsInChildren<Image>();
        _healthDisplayAnimator = healthDisplay.GetComponent<Animator>();
    }

    public override void Start()
    {
        LoadTemp();
    }

    #endregion

    private void UpdateHealthBar()
    {
        if (Health < 0) return;
        for (var i = 0; i < Health; i++) _healthIcons[i].gameObject.SetActive(true);
        for (var i = Health; i < maxHealth; i++) _healthIcons[i].gameObject.SetActive(false);
    }

    private void UpdateFuelBar()
    {
        if (Fuel < 0f) return;
        fuelBar.fillAmount = Fuel;
    }

    #region Save & Load Methods
    public void SaveTemp()
    {
        PlayerPrefs.SetInt(HealthTempKey, Health);
        PlayerPrefs.SetInt(TokenTempKey, Token);
        PlayerPrefs.SetFloat(FuelTempKey, Fuel);
        PlayerPrefs.SetInt(LevelIndexKey, PlayerPrefs.GetInt(LevelIndexKey, 0) + 1);
    }

    private void LoadTemp()
    {
        Health = PlayerPrefs.GetInt(HealthTempKey, maxHealth);
        Token = PlayerPrefs.GetInt(TokenTempKey, 0);
        Fuel = PlayerPrefs.GetFloat(FuelTempKey, maxFuel);
    }

    public void ClearTemp()
    {
        PlayerPrefs.SetInt(HealthTempKey, maxHealth);
        PlayerPrefs.SetInt(TokenTempKey, 0);
        PlayerPrefs.SetFloat(FuelTempKey, maxFuel);
        PlayerPrefs.SetInt(LevelIndexKey, 0);
    }

    #endregion
}