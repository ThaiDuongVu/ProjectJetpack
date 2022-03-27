using UnityEngine;

public class Fuel : Collectible
{
    [SerializeField] private float reward = 20f;

    public override void Collect(Transform target)
    {
        if (IsCollected || !CanBeCollected) return;

        var player = target.GetComponent<Player>();
        if (player.PlayerResources.Fuel > player.PlayerResources.maxFuel - reward) return;

        base.Collect(target);
        player.PlayerResources.Fuel += reward;
    }
}