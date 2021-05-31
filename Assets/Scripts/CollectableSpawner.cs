using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private Collectable collectable;
    public int MinSpawnAmount { get; set; } = 3;
    public int MaxSpawnAmount { get; set; } = 8;
    public float SpawnRadius { get; set; } = 6f;

    /// <summary>
    /// Spawn assigned collectables.
    /// </summary>
    public void Spawn()
    {
        for (int i = 0; i < Random.Range(MinSpawnAmount, MaxSpawnAmount); i++)
            Instantiate(collectable, transform.position, transform.rotation).GetComponent<Collectable>().InitPosition =
                (Vector2)transform.position + new Vector2(Random.Range(-SpawnRadius, SpawnRadius), Random.Range(-SpawnRadius, SpawnRadius));
    }
}
