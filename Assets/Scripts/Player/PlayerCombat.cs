using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Player player;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        player = GetComponent<Player>();
    }

    /// <summary>
    /// Deal damage to an enemy.
    /// </summary>
    /// <param name="enemy">Enemy to deal damage to</param>
    /// <param name="damage">Damage to deal</param>
    public void DealDamage(Enemy enemy, float damage)
    {
        var slice = Instantiate(player.slicePrefab, player.transform.position, player.transform.rotation).GetComponent<LineRenderer>();
        slice.SetPosition(0, player.transform.position + Vector3.up * 0.5f);
        slice.SetPosition(1, player.transform.position + player.transform.forward * player.Movement.DashDistance / 2f);
        slice.SetPosition(2, player.transform.position + player.transform.forward * player.Movement.DashDistance);

        player.Combo.Add(1);

        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
    }
}
