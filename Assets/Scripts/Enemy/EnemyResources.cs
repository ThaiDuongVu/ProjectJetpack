using UnityEngine;

public class EnemyResources : MonoBehaviour
{
    public float CurrentHealth { get; set; }
    public float MaxHealth;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        
    }
}
