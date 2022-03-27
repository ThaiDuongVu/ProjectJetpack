using UnityEngine;

public class Health : Collectible
{
    [SerializeField] public int reward = 1;

    public override void Collect(Transform target)
    {
        if (IsCollected || !CanBeCollected) return;

        var player = target.GetComponent<Player>();
        if (player.PlayerResources.Health > player.PlayerResources.maxHealth - reward) return;

        base.Collect(target);
        player.PlayerResources.Health += reward;
    }
}