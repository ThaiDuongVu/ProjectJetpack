using UnityEngine;

public class Heart : Collectible
{
    public int reward = 1;

    protected override void OnCollected(Transform target)
    {
        if (IsCollected || !CanBeCollected) return;

        var player = target.GetComponent<Player>();
        if (player.PlayerResources.Health > player.PlayerResources.maxHealth - reward) return;

        base.OnCollected(target);
        player.PlayerResources.Health += reward;
    }
}
