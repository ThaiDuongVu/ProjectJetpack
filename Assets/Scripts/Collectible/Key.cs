using UnityEngine;

public class Key : Collectible
{
    public string[] ids;

    public override void OnCollected(Transform target)
    {
        if (IsCollected || !CanBeCollected) return;

        var player = target.GetComponent<Player>();
        foreach (var id in ids) player.CollectKey(id);

        base.OnCollected(target);
    }
}
