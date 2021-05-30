using UnityEngine;

public class Coin : Collectable
{
    /// <summary>
    /// Add to player coins.
    /// </summary>
    public override void OnCollected()
    {
        if (isCollected) return;

        Player.Instance.Resources.CollectedCoins += 1 * Player.Instance.Combo.Multiplier;
        animator.SetTrigger("collect");
        destroyDelay.enabled = true;
        isCollected = true;
    }
}
