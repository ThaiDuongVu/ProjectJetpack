using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private ParticleSystem explosionRedPrefab;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Die()
    {
        if (IsDead) return;

        Instantiate(explosionRedPrefab, transform.position, Quaternion.identity);

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
