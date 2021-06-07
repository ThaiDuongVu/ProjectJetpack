using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private bool isPracticeDummy;
    [SerializeField] private Light2D light2D;
    private CircleCollider2D[] circleColliders2D;
    private ShadowCaster2D shadowCaster2D;
    private Animator animator;
    private CollectableSpawner collectableSpawner;

    public float CurrentHealth { get; set; }
    public float MaxHealth;

    public bool IsDead { get; set; }
    public bool IsRagdoll { get; set; }
    public bool IsKnockingBack { get; set; }

    public bool IsStaggered { get; set; }
    private const float StaggerDuration = 0.5f;

    [SerializeField] private Transform[] rig;
    private Vector2[] ragdollPositions;
    public const float RagdollRange = 4f;
    public const float RagdollInterpolationRatio = 0.025f;

    [SerializeField] private ParticleSystem bloodSpatPrefab;

    private DelayDestroyer delayDestroyer;

    private Vector2 knockBackPosition;
    private Vector2 knockBackDirection;
    private const float KnockBackDistance = 5f;
    private const float KnockBackEpsilon = 1f;
    private const float KnockBackInterpolationRatio = 0.2f;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        circleColliders2D = GetComponents<CircleCollider2D>();
        shadowCaster2D = GetComponent<ShadowCaster2D>();
        animator = GetComponent<Animator>();
        collectableSpawner = GetComponent<CollectableSpawner>();

        CurrentHealth = MaxHealth;
        ragdollPositions = new Vector2[rig.Length];

        delayDestroyer = GetComponent<DelayDestroyer>();
        delayDestroyer.enabled = false;
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        if (IsRagdoll) Ragdoll();
        if (IsKnockingBack) KnockBack();
    }

    /// <summary>
    /// Deal damage to enemy.
    /// </summary>
    /// <param name="damage">Damage to deal</param>
    void IDamageable.TakeDamage(float damage, Vector2 direction)
    {
        knockBackDirection = direction;

        if (isPracticeDummy)
        {
            StartCoroutine(Stagger());
            return;
        }


        CurrentHealth -= damage;
        if (CurrentHealth <= 0f)
            (this as IDamageable).Die();
        else
            StartCoroutine(Stagger());
    }

    /// <summary>
    /// Handle enemy death.
    /// </summary>
    void IDamageable.Die()
    {
        IsDead = true;

        StartCoroutine(GameController.Instance.SlowDownEffect());
        Instantiate(bloodSpatPrefab, transform.position, Player.Instance.transform.rotation);
        EnableRagdoll();
        collectableSpawner.Spawn();
        delayDestroyer.enabled = true;
        EnemySpawner.Instance.CurrentPopulation--;
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
        foreach (CircleCollider2D collider in circleColliders2D) collider.enabled = false;
        shadowCaster2D.enabled = false;
        animator.SetTrigger("die");

        // Update enemy state
        IsRagdoll = true;
    }

    /// <summary>
    /// Move body parts into ragdoll position.
    /// </summary>
    private void Ragdoll()
    {
        for (int i = 0; i < rig.Length; i++)
            rig[i].position = Vector2.Lerp(rig[i].position, ragdollPositions[i], RagdollInterpolationRatio);
    }

    /// <summary>
    /// Attack behaviour for enemy.
    /// To be implemented in child class.
    /// </summary>
    public virtual void Attack() { }

    /// <summary>
    /// Set knock back position and enable it.
    /// </summary>
    private void StartKnockBack()
    {
        knockBackPosition = (Vector2)transform.position + knockBackDirection * KnockBackDistance;
        IsKnockingBack = true;
    }

    /// <summary>
    /// Knock enemy backwards.
    /// </summary>
    private void KnockBack()
    {
        transform.position = Vector2.Lerp(transform.position, knockBackPosition, KnockBackInterpolationRatio);
        if (((Vector2)transform.position - knockBackPosition).magnitude <= KnockBackEpsilon)
        {
            Instantiate(bloodSpatPrefab, transform.position, Player.Instance.transform.rotation);
            IsKnockingBack = false;
        }
    }

    /// <summary>
    /// Add stagger effects to player.
    /// </summary>
    private IEnumerator Stagger()
    {
        IsStaggered = true;
        StartKnockBack();
        animator.SetTrigger("enterStagger");

        yield return new WaitForSeconds(StaggerDuration);

        IsStaggered = false;
        animator.SetTrigger("exitStagger");
        animator.ResetTrigger("enterStagger");
    }
}