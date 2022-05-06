using UnityEngine;
using TMPro;

public class VendingMachineCustomization : VendingMachine
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private int id;
    private bool _isUnlocked;
    private bool _isEquipped;

    #region Unity Event

    public override void Start()
    {
        base.Start();

        UpdateText();
    }

    #endregion

    public void UpdateText()
    {
        // 0: Locked
        // 1: Unlocked
        _isUnlocked = PlayerPrefs.GetInt($"{Player.CustomizationUnlockKey}{id.ToString()}", 0) == 1;
        _isEquipped = PlayerPrefs.GetInt($"{Player.CustomizationEquipKey}", 0) == id;

        if (_isUnlocked) text.text = _isEquipped ? "Equipped" : "Unlocked";
        else text.text = price.ToString();
    }

    public override void Purchase(Player player)
    {
        if (player.PlayerResources.Token < price && !_isUnlocked)
        {
            GameController.Instance.SendUIMessage("Not enough tokens!");
            return;
        }

        if (!_isUnlocked)
        {
            player.PlayerResources.Token -= price;
            PlayerPrefs.SetInt(Player.TokensKey, player.PlayerResources.Token);
            PlayerPrefs.SetInt($"{Player.CustomizationUnlockKey}{id.ToString()}", 1);

            _isUnlocked = true;
        }

        if (_isUnlocked)
        {
            PlayerPrefs.SetInt($"{Player.CustomizationEquipKey}", id);
            AudioController.Instance.Play(AudioVariant.UnlockCustomization);
        }

        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }
}
