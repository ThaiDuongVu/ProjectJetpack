using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    #region Singleton

    private static LevelGenerator _levelGeneratorInstance;

    public static LevelGenerator Instance
    {
        get
        {
            if (_levelGeneratorInstance == null) _levelGeneratorInstance = FindObjectOfType<LevelGenerator>();
            return _levelGeneratorInstance;
        }
    }

    #endregion

    public LevelVariant Variant { get; private set; }
    public int LevelIndex { get; set; }

    [SerializeField] private Transform platformsParent;
    private Platform[] _platformPrefabs;
    private Enemy[] _groundEnemyPrefabs;
    private Enemy[] _wallEnemyPrefabs;
    private Enemy[] _floatingEnemyPrefabs;

    [SerializeField] private Vector2Int blockDistanceRange;
    private const int MinBlock = -82;
    private int _currentBlock;

    [SerializeField] private float floatingEnemySpawnProbability = 0.2f;
    [SerializeField] private float wallEnemySpawnProbability = 0.2f;

    [SerializeField] private GameObject marketplacePrefab;

    private GameObject[] _bossLevelPrefabs;

    #region Unity Event

    private void Awake()
    {
        _platformPrefabs = Resources.LoadAll<Platform>("Levels/Platforms");

        _groundEnemyPrefabs = Resources.LoadAll<Enemy>("Enemies/Ground");
        _wallEnemyPrefabs = Resources.LoadAll<Enemy>("Enemies/Wall");
        _floatingEnemyPrefabs = Resources.LoadAll<Enemy>("Enemies/Floating");

        _bossLevelPrefabs = Resources.LoadAll<GameObject>("Levels/BossLevels");
    }

    private void Start()
    {
        Generate();
    }

    #endregion

    private void Generate()
    {
        // TODO: Generate the first platform and spawn the player

        LevelIndex = PlayerPrefs.GetInt(PlayerResources.LevelIndexKey, 0);

        switch (LevelIndex)
        {
            case >= 0 and <= 1:
                GenerateRegular();
                break;
            case 2:
                GenerateMarketplace();
                break;
            case 3:
                // One-eye Spider boss
                GenerateBoss(0);
                break;
            case >= 4 and <= 6:
                GenerateRegular();
                break;
            case 7:
                GenerateMarketplace();
                break;
            case 8:
                // Shielded Fly boss
                GenerateBoss(1);
                break;
            case >= 9 and <= 12:
                GenerateRegular();
                break;
            case 13:
                GenerateMarketplace();
                break;
            case 14:
                // Rocket Rat boss
                GenerateBoss(2);
                break;
            case >= 15 and <= 19:
                GenerateRegular();
                break;
            case 20:
                GenerateMarketplace();
                break;
            case 21:
                // Three-headed Dragon boss
                GenerateBoss(3);
                break;
            case >= 22 and <= 26:
                GenerateRegular();
                break;
            case 27:
                GenerateMarketplace();
                break;
            case 28:
                // TODO: Final boss fight
                GenerateBoss(4);
                break;
            default:
                PlayGameCompleteAnimation();
                break;
        }
    }

    private Platform SpawnPlatform()
    {
        var platform = _platformPrefabs[Random.Range(0, _platformPrefabs.Length)];
        var spawnPosition = new Vector2(platform.xPositions[Random.Range(0, platform.xPositions.Length)], _currentBlock);

        Instantiate(platform, spawnPosition, Quaternion.identity).transform.parent = platformsParent;
        // Spawn ground enemies
        for (var i = 0; i < platform.spawnEnemyCount; i++)
        {
            // Only spawn enemy if the probability checks out
            if (Random.Range(0f, 1f) > platform.spawnEnemyProbability) continue;

            var spawnEnemy = _groundEnemyPrefabs[Random.Range(0, _groundEnemyPrefabs.Length)];
            var spawnEnemyPosition =
                new Vector2(spawnPosition.x + Random.Range(-platform.size.x / 2f + 0.5f, platform.size.x / 2f - 0.5f), spawnPosition.y + 1f);
            Instantiate(spawnEnemy, spawnEnemyPosition, Quaternion.identity);
        }

        return platform;
    }

    private void SpawnFloatingEnemy()
    {
        if (Random.Range(0f, 1f) > floatingEnemySpawnProbability) return;

        var spawnEnemy = _floatingEnemyPrefabs[Random.Range(0, _floatingEnemyPrefabs.Length)];
        var spawnEnemyPosition = new Vector2(0f, _currentBlock - Random.Range(blockDistanceRange.x / 2, blockDistanceRange.y / 2 - 1));

        Instantiate(spawnEnemy, spawnEnemyPosition, Quaternion.identity);
    }

    private void SpawnWallEnemy()
    {
        if (Random.Range(0f, 1f) > wallEnemySpawnProbability) return;

        var spawnEnemy = _wallEnemyPrefabs[Random.Range(0, _wallEnemyPrefabs.Length)];
        var spawnEnemyPosition =
            new Vector2(spawnEnemy.transform.position.x, _currentBlock - Random.Range(blockDistanceRange.x / 2, blockDistanceRange.y / 2 - 1));

        Instantiate(spawnEnemy, spawnEnemyPosition, spawnEnemy.transform.rotation);
    }

    private void GenerateRegular()
    {
        Variant = LevelVariant.Regular;

        while (_currentBlock > MinBlock)
        {
            _currentBlock -= SpawnPlatform().size.y;
            if (Random.Range(-1f, 1f) > 0f) SpawnFloatingEnemy();
            else SpawnWallEnemy();
            _currentBlock -= Random.Range(blockDistanceRange.x, blockDistanceRange.y);
        }
    }

    private void GenerateMarketplace()
    {
        Variant = LevelVariant.Marketplace;
        Instantiate(marketplacePrefab, Vector2.zero, Quaternion.identity).transform.parent = transform;
    }

    private void GenerateBoss(int index)
    {
        Variant = LevelVariant.Boss;
        if (!_bossLevelPrefabs[index]) return;

        Instantiate(_bossLevelPrefabs[index], Vector2.zero, Quaternion.identity).transform.parent = transform;
    }

    private void PlayGameCompleteAnimation()
    {
        // TODO: Play game complete animation
    }
}
