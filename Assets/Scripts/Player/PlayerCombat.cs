using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private Player player;
    public int ComboToAdd { get; set; } = 1;

    public float Damage { get; set; } = 1f;

    private InputManager inputManager;

    /// <summary>
    /// Unity Event function.
    /// Initialize input handler on object enabled.
    /// </summary>
    private void OnEnable()
    {
        inputManager = new InputManager();

        inputManager.Enable();
    }

    #region Input Methods

    #endregion

    /// <summary>
    /// Unity Event function.
    /// Disable input handling on object disabled.
    /// </summary>
    private void OnDisable()
    {
        inputManager.Disable();
    }

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        player = Player.Instance;
    }

    /// <summary>
    /// Deal damage to an enemy.
    /// </summary>
    /// <param name="enemy">Enemy to damage</param>
    public void DealDamage(Enemy enemy)
    {
        (enemy as IDamageable).TakeDamage(Damage, transform.up);
        // Add player combo
        player.Combo.Add(ComboToAdd);

        // Combat flavours
        Instantiate(player.dashSlicePrefab, player.transform.position, player.transform.rotation);
        // Instantiate(player.bloodSpatPrefab, enemy.transform.position, player.transform.rotation);
        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
    }
}
