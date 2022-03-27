using UnityEngine;

public class Token : Collectible
{
    [SerializeField] private int reward = 1;

    public override void Collect(Transform target)
    {
        if (IsCollected || !CanBeCollected) return;
        base.Collect(target);

        var player = target.GetComponent<Player>();
        player.PlayerResources.Token += player.PlayerCombo.Multiplier >= 1 ? reward * player.PlayerCombo.Multiplier : reward;
    }
}