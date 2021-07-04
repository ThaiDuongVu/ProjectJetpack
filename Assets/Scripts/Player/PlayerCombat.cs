using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Player player;
    public int ComboToAdd { get; set; } = 1;
    public float Damage { get; set; } = 1f;

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
    /// <param name="enemy">Enemy to damage</param>
    public void DealDamage(Enemy enemy)
    {
        enemy.TakeDamage(Damage, transform.up);
        // Add player combo
        player.Combo.Add(ComboToAdd);

        // Combat flavours
        Instantiate(player.slicePrefab, player.transform.position, player.transform.rotation);
        Instantiate(player.bloodSpatPrefab, enemy.transform.position, player.transform.rotation);
        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
    }
}
