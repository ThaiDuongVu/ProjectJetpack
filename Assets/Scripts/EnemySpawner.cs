using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Singleton

    private static EnemySpawner _enemySpawnerInstance;

    public static EnemySpawner Instance
    {
        get
        {
            if (_enemySpawnerInstance == null) _enemySpawnerInstance = FindObjectOfType<EnemySpawner>();
            return _enemySpawnerInstance;
        }
    }

    #endregion

    [SerializeField] private Enemy[] regularEnemyPrefabs;
    [SerializeField] private Enemy[] midEnemyPrefabs;

    public void SpawnRegular()
    {
        var spawnEnemy = regularEnemyPrefabs[Random.Range(0, regularEnemyPrefabs.Length)];
        var spawnPosition = new Vector2(Random.Range(spawnEnemy.minPosition.x, spawnEnemy.maxPosition.x),
                                        Random.Range(spawnEnemy.minPosition.y, spawnEnemy.maxPosition.y));
        var spawnRotation = Quaternion.identity;

        Instantiate(spawnEnemy, spawnPosition, spawnRotation);
    }

    public void SpawnMid()
    {
        var spawnEnemy = midEnemyPrefabs[Random.Range(0, midEnemyPrefabs.Length)];
        var spawnPosition = new Vector2(Random.Range(spawnEnemy.minPosition.x, spawnEnemy.maxPosition.x),
                                        Random.Range(spawnEnemy.minPosition.y, spawnEnemy.maxPosition.y));
        var spawnRotation = Quaternion.identity;

        Instantiate(spawnEnemy, spawnPosition, spawnRotation);
    }
}
