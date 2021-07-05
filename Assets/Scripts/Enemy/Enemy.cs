using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    public EnemyResources Resources { get; set; }
    public DelayDestroyer DelayDestroyer { get; set; }
    public Animator Animator { get; set; }
    public CircleCollider2D[] CircleColliders2D { get; set; }
    public ShadowCaster2D ShadowCaster2D { get; set; }

    public bool IsDead { get; set; }
    public bool IsRagdoll { get; set; }
    public bool IsKnockingBack { get; set; }

    public bool IsStaggered { get; set; }
    private const float StaggerDuration = 0.5f;

    [SerializeField] private Transform rig;
    [SerializeField] private Light2D light2D;

    private Transform[] ragdolls;
    private Vector2[] ragdollPositions;
    public const float RagdollRange = 4f;
    public const float RagdollInterpolationRatio = 0.025f;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        Resources = GetComponent<EnemyResources>();
        DelayDestroyer = GetComponent<DelayDestroyer>();
        Animator = GetComponent<Animator>();
        CircleColliders2D = GetComponents<CircleCollider2D>();
        ShadowCaster2D = GetComponent<ShadowCaster2D>();

        ragdolls = rig.GetComponentsInChildren<Transform>();
        ragdollPositions = new Vector2[ragdolls.Length];
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        DelayDestroyer.enabled = false;
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        if (IsRagdoll) Ragdoll();
    }

    /// <summary>
    /// Deal damage to enemy.
    /// </summary>
    /// <param name="damage">Amount of damage to deal</param>
    /// <param name="direction">Direction to deal damage</param>
    public void TakeDamage(float damage, Vector2 direction)
    {
        Resources.CurrentHealth -= damage;

        if (Resources.CurrentHealth <= 0f) Die();
    }

    /// <summary>
    /// Handle enemy death.
    /// </summary>
    public void Die()
    {
        EnableRagdoll();
        GameController.Instance.StartCoroutine(GameController.Instance.SlowDownEffect());
        DelayDestroyer.enabled = true;
    }

    /// <summary>
    /// Enable enemy ragdoll.
    /// </summary>
    public void EnableRagdoll()
    {
        // Generate ragdoll positions for each body parts
        for (int i = 0; i < ragdollPositions.Length; i++)
            ragdollPositions[i] = (Vector2)transform.position + new Vector2(Random.Range(-RagdollRange, RagdollRange), Random.Range(-RagdollRange, RagdollRange));

        // Disable light, collider, shadows and animations
        light2D.enabled = false;
        ShadowCaster2D.enabled = false;
        foreach (CircleCollider2D collider in CircleColliders2D) collider.enabled = false;
        Animator.SetTrigger("die");

        // Update enemy state
        IsRagdoll = true;
    }

    /// <summary>
    /// Move body parts into ragdoll position.
    /// </summary>
    private void Ragdoll()
    {
        for (int i = 0; i < ragdolls.Length; i++)
            ragdolls[i].position = Vector2.Lerp(ragdolls[i].position, ragdollPositions[i], RagdollInterpolationRatio);
    }
}
