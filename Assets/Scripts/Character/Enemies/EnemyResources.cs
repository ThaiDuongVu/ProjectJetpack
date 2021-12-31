using UnityEngine;

public class EnemyResources : MonoBehaviour
{
    public Enemy Enemy { get; private set; }

    private static readonly int IsStaggerAnimationTrigger = Animator.StringToHash("isStagger");

    [SerializeField] private float maxHealth;
    private float currentHealth;
    public float Health
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            if (value <= 0f) Enemy.Die();
        }
    }

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        Enemy = GetComponent<Enemy>();
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        Health = maxHealth;
    }
}
