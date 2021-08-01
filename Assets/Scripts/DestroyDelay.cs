using System.Collections;
using UnityEngine;

public class DestroyDelay : MonoBehaviour
{
    [SerializeField] private float delayDuration;
    private WaitForSeconds delay;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        delay = new WaitForSeconds(delayDuration);
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        StartCoroutine(Destroy());
    }

    /// <summary>
    /// Destroy after the delay.
    /// </summary>
    private IEnumerator Destroy()
    {
        yield return delay;
        Destroy(gameObject);
    }
}
