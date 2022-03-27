using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [SerializeField] private Collectible collectiblePrefab;
    public Vector2Int spawnAmountRange;

    public void Spawn()
    {
        for (var i = 0; i < Random.Range(spawnAmountRange.x, spawnAmountRange.y); i++)
            Instantiate(collectiblePrefab, transform.position, Quaternion.identity);
    }
}