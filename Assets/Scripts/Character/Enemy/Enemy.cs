using UnityEngine;

public class Enemy : Character
{
    protected CollectibleSpawner[] collectibleSpawners;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();
    }

    #endregion

    public override void Die()
    {
        base.Die();

        collectibleSpawners = GetComponentsInChildren<CollectibleSpawner>();
        foreach (var spawner in collectibleSpawners) spawner.Spawn();
        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("BasePlatform")) Die();
    }
}
