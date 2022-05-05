using UnityEngine;

public class Enemy : Character
{
    private CollectibleSpawner[] _collectibleSpawners;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _collectibleSpawners = GetComponentsInChildren<CollectibleSpawner>();
    }

    #endregion

    public override void Die()
    {
        base.Die();

        foreach (var spawner in _collectibleSpawners) spawner.Spawn();
        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("BasePlatform")) Die();
    }
}
