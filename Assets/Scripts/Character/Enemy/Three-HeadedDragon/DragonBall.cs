using UnityEngine;

public class DragonBall : Enemy
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockBackForce = 10f;

    public override void Die()
    {
        // base.Die();

        CollectibleSpawners = GetComponentsInChildren<CollectibleSpawner>();
        foreach (var spawner in CollectibleSpawners) spawner.Spawn();

        if (explosionPrefab) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        AudioController.Instance.Play(AudioVariant.Explode1);

        Destroy(gameObject);
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            var player = other.transform.GetComponent<Player>();
            player.TakeDamage(damage);
            player.KnockBack((player.transform.position - transform.position).normalized, knockBackForce);
        }

        Die();
    }
}
