using UnityEngine;
using System.Collections;

public class DelayedDestroyer : MonoBehaviour
{
    public float delayTime;
    public bool destroyOnStartup;

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        if (destroyOnStartup) StartCoroutine(Destroy());
    }

    /// <summary>
    /// Destroy current game object after a delay.
    /// </summary>
    public IEnumerator Destroy()
    {
        yield return new WaitForSeconds(delayTime);

        Destroy(gameObject);
    }
}