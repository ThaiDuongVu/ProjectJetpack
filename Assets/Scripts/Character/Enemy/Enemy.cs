using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private ParticleSystem explosionRedPrefab;
    private CollectibleSpawner[] _collectibleSpawners;

    public override void Awake()
    {
        base.Awake();

        _collectibleSpawners = GetComponentsInChildren<CollectibleSpawner>();
    }

    public override void Die()
    {
        if (IsDead) return;

        Instantiate(explosionRedPrefab, transform.position, Quaternion.identity);
        foreach (var spawner in _collectibleSpawners) spawner.Spawn();
        
        GameController.Instance.StartCoroutine(GameController.Instance.SlowDownEffect());
        Destroy(gameObject);

        IsDead = true;
    }

    public void DieImmediate()
    {
        if (IsDead) return;

        Instantiate(explosionRedPrefab, transform.position, Quaternion.identity);

        GameController.Instance.StartCoroutine(GameController.Instance.SlowDownEffect());
        Destroy(gameObject);

        IsDead = true;
    }
}
