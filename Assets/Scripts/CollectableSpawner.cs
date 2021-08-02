using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private Collectable collectablePrefab;

    private const int MinSpawnNumber = 3;
    private const int MaxSpawnNumber = 6;
    private const float SpawnRange = 5f;

    /// <summary>
    /// Spawn some collectable prefab.
    /// </summary>
    public void Spawn(int min = MinSpawnNumber, int max = MaxSpawnNumber, float range = SpawnRange)
    {
        for (int i = 0; i < Random.Range(min, max); i++) Instantiate(collectablePrefab, transform.position, Quaternion.identity);
    }
}
