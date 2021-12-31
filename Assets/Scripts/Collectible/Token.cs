using UnityEngine;

public class Token : Collectible
{
    /// <summary>
    /// On object collected by another object.
    /// </summary>
    /// <param name="target">Collect target</param>
    public override void OnCollected(Transform target)
    {
	    if (isCollected) return;
        base.OnCollected(target);

        var player = target.GetComponent<Player>();
        player.Resources.Token += player.Combo.Multiplier;
    }
}
