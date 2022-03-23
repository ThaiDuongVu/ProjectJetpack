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

    [Header("Fuel")]
    [SerializeField] private Image fuelBar;
    public float maxFuel = 100f;
    private float _fuel;
    public float Fuel
    {
        get => _fuel;
        set
        {
            _fuel = value;
            UpdateFuelBar();
        }
    }

    [Header("Token")]
    [SerializeField] private TMP_Text tokenText;
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
        base.Start();

        Fuel = maxFuel;
    }

    #endregion

    private void UpdateHealthBar()
    {
        if (Health < 0) return;
        for (int i = 0; i < Health; i++) _healthIcons[i].gameObject.SetActive(true);
        for (int i = Health; i < maxHealth; i++) _healthIcons[i].gameObject.SetActive(false);
    }

    private void UpdateFuelBar()
    {
        if (Fuel < 0f) return;
        fuelBar.transform.localScale = new Vector2(Fuel / maxFuel, 1f);
    }
}