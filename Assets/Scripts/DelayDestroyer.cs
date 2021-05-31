using System.Collections;
using UnityEngine;

public class DelayDestroyer : MonoBehaviour
{
    [SerializeField] private float delay;

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        StartCoroutine(DestroyAfterSeconds());
    }

    /// <summary>
    /// Wait for [delay] seconds and destroy current object.
    /// </summary>
    private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
