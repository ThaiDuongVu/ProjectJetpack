using UnityEngine;

public class Humanoid : MonoBehaviour
{
    private Enemy enemy;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        
    }
}
