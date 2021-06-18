using System.Collections;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public bool DelayedDone { get; set; }
    public Vector2 InitPosition { get; set; }
    private const float CollectInterpolatioRatio = 0.2f;
    private const float InitInterpolationRatio = 0.025f;
    private const float LerpThreshold = 5f;
    private const float CollectDelay = 0.5f;
    protected bool isCollected;
    protected Animator animator;
    protected DelayDestroyer delayDestroyer;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();
        delayDestroyer = GetComponent<DelayDestroyer>();
        delayDestroyer.enabled = false;
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        StartCoroutine(WaitCollect());
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        if ((transform.position - Player.Instance.transform.position).magnitude <= LerpThreshold && DelayedDone) OnCollected();
        transform.position = Vector2.Lerp(transform.position,
                                          DelayedDone ? (Vector2)Player.Instance.transform.position : InitPosition,
                                          DelayedDone ? CollectInterpolatioRatio : InitInterpolationRatio);
    }

    /// <summary>
    /// Wait for delay then start lerping to player.
    /// </summary>
    private IEnumerator WaitCollect()
    {
        yield return new WaitForSeconds(CollectDelay);
        DelayedDone = true;
    }

    /// <summary>
    /// Add to player property on item collected.
    /// </summary>
    public virtual void OnCollected() { }
}
