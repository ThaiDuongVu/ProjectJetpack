using UnityEngine;

public class Enemy : Character
{
    protected CollectibleSpawner[] CollectibleSpawners;

    public override void Die()
    {
        base.Die();

        CollectibleSpawners = GetComponentsInChildren<CollectibleSpawner>();
        foreach (var spawner in CollectibleSpawners) spawner.Spawn();
        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("BasePlatform")) Die();
    }
}
