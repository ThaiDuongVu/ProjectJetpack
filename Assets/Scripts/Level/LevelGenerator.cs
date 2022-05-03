using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform platformsParent;
    private Platform[] platformPrefabs;
    private Enemy[] groundEnemyPrefabs;
    private Enemy[] wallEnemyPrefabs;
    private Enemy[] floatingEnemyPrefabs;

    [SerializeField] private Vector2Int blockDistanceRange;
    private const int MinBlock = -82;
    private int _currentBlock = 0;

    [SerializeField] private float floatingEnemySpawnProbability = 0.2f;

    #region Unity Event

    private void Awake()
    {
        platformPrefabs = Resources.LoadAll<Platform>("Levels/Platforms");

        groundEnemyPrefabs = Resources.LoadAll<Enemy>("Enemies/Ground");
        wallEnemyPrefabs = Resources.LoadAll<Enemy>("Enemies/Wall");
        floatingEnemyPrefabs = Resources.LoadAll<Enemy>("Enemies/Floating");
    }

    private void Start()
    {
        Generate();
    }

    #endregion

    private void Generate()
    {
        // TODO: Generate the first platform and spawn the player

        while (_currentBlock > MinBlock)
        {
            _currentBlock -= SpawnPlatform().size.y;

            SpawnFloatingEnemy();

            _currentBlock -= Random.Range(blockDistanceRange.x, blockDistanceRange.y);
        }
    }

    private Platform SpawnPlatform()
    {
        var platform = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
        var spawnPosition = new Vector2(platform.xPositions[Random.Range(0, platform.xPositions.Length)], _currentBlock);

        Instantiate(platform, spawnPosition, Quaternion.identity).transform.parent = platformsParent;
        // Spawn ground enemies
        for (int i = 0; i < platform.spawnEnemyCount; i++)
        {
            // Only spawn enemy if the probability checks out
            if (Random.Range(0f, 1f) > platform.spawnEnemyProbability) continue;

            var spawnEnemy = groundEnemyPrefabs[Random.Range(0, groundEnemyPrefabs.Length)];
            var spawnEnemyPosition =
                new Vector2(spawnPosition.x + Random.Range(-platform.size.x / 2f + 0.5f, platform.size.x / 2f - 0.5f), spawnPosition.y + 1f);
            Instantiate(spawnEnemy, spawnEnemyPosition, Quaternion.identity);
        }

        return platform;
    }

    private void SpawnFloatingEnemy()
    {
        if (Random.Range(0f, 1f) > floatingEnemySpawnProbability) return;

        var spawnEnemy = floatingEnemyPrefabs[Random.Range(0, floatingEnemyPrefabs.Length)];
        var spawnEnemyPosition = new Vector2(0f, _currentBlock - Random.Range(blockDistanceRange.x / 2, blockDistanceRange.y / 2 - 1));

        Instantiate(spawnEnemy, spawnEnemyPosition, Quaternion.identity);
    }
}
