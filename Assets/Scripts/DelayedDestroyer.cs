using UnityEngine;
using System.Collections;

public class DelayedDestroyer : MonoBehaviour
{
    public float delayTime;
    public bool destroyOnStartup;
    
    private void Start()
    {
        if (destroyOnStartup) StartCoroutine(Destroy());
    }
    
    public IEnumerator Destroy()
    {
        yield return new WaitForSeconds(delayTime);

        Destroy(gameObject);
    }
}