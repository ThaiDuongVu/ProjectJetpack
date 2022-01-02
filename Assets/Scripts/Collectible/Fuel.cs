using UnityEngine;

public class Fuel : Collectible
{
    public float reward = 5f;

    /// <summary>
    /// On object collected by another object.
    /// </summary>
    /// <param name="target">Collect target</param>
    public override void OnCollected(Transform target)
    {
	    if (isCollected) return;

        var player = target.GetComponent<Player>();
        if (player.Resources.Fuel > player.Resources.maxFuel - 5f) return;

        base.OnCollected(target);
        player.Resources.Fuel += reward;
    }
}
