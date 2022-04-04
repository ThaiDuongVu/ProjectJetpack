using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [SerializeField] private Collectible collectiblePrefab;
    public Vector2Int spawnAmountRange;
    [SerializeField] private Vector2 initPositionRange = new Vector2(1f, 1f);

    /// <summary>
    /// Spawn a number of collectibles from current object.
    /// </summary>
    public void Spawn()
    {
        for (var i = 0; i < Random.Range(spawnAmountRange.x, spawnAmountRange.y); i++)
            Instantiate(collectiblePrefab, transform.position, Quaternion.identity).InitPosition =
                (Vector2)transform.position + new Vector2(Random.Range(-initPositionRange.x, initPositionRange.x),
                                                          Random.Range(-initPositionRange.y, initPositionRange.y));
    }
}