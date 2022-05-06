using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [SerializeField] private Collectible collectiblePrefab;
    [SerializeField] private Vector2Int spawnAmountRange;
    private const float InitForce = 15f;

    public void Spawn()
    {
        for (var i = 0; i < Random.Range(spawnAmountRange.x, spawnAmountRange.y); i++)
        {
            Instantiate(collectiblePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>()
                .AddForce(new Vector2(Random.Range(-1f, 1f), 1f).normalized * InitForce, ForceMode2D.Impulse);
        }
    }
}
