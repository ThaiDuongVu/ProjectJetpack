using UnityEngine;

public class VendingMachineJetpack : VendingMachine
{
    [SerializeField] private int jetpackIndex;

    public override void Purchase(Player player)
    {
        if (player.PlayerResources.Token < price)
        {
            GameController.Instance.SendUIMessage("Not enough tokens!");
            return;
        }

        if (PlayerPrefs.GetInt(PlayerCombat.JetpackKey, 0) == jetpackIndex)
        {
            GameController.Instance.SendUIMessage("Jetpack already equipped");
            return;
        }

        PlayerPrefs.SetInt(PlayerCombat.JetpackKey, jetpackIndex);
        player.PlayerResources.Token -= price;
        player.PlayerCombat.UpdateJetpack();

        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }
}
