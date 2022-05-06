using UnityEngine;

public class Token : Collectible
{
    public int reward = 1;

    protected override void OnCollected(Transform target)
    {
        if (IsCollected || !CanBeCollected) return;
        base.OnCollected(target);

        var player = target.GetComponent<Player>();
        player.PlayerResources.Token += player.PlayerCombo.Multiplier > 0 ? player.PlayerCombo.Multiplier * reward : reward;
    }
}
