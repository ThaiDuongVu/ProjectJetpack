using UnityEngine;

public class Fuel : Collectible
{
    public float reward = 0.1f;

    public override void OnCollected(Transform target)
    {
        if (IsCollected || !CanBeCollected) return;

        var player = target.GetComponent<Player>();
        if (player.PlayerResources.Fuel > player.PlayerResources.maxFuel - reward) return;

        base.OnCollected(target);
        player.PlayerResources.Fuel += reward;
    }
}
