using UnityEngine;

public class DestructablePlatform : Platform
{
    [SerializeField] private ParticleSystem explosionPrefab;

    public void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());
        Destroy(gameObject);
    }
}
