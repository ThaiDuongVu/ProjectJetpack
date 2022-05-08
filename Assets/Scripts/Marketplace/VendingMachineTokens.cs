using UnityEngine;

public class VendingMachineTokens : VendingMachine
{
    public override void Purchase(Player player)
    {
        if (player.PlayerResources.Health < price + 1)
        {
            GameController.Instance.SendUIMessage("Not enough hearts!");
            return;
        }

        foreach (var collectible in collectibleSpawners) collectible.Spawn();
        player.PlayerResources.Health -= price;

        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }
}
