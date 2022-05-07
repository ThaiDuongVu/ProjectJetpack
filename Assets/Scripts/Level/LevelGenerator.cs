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

    [SerializeField] private Transform platformsParent;
    private Platform[] _platformPrefabs;
    private Enemy[] _groundEnemyPrefabs;
    private Enemy[] _wallEnemyPrefabs;
    private Enemy[] _floatingEnemyPrefabs;

    [SerializeField] private Vector2Int blockDistanceRange;
    private const int MinBlock = -82;
    private int _currentBlock;

    [SerializeField] private float floatingEnemySpawnProbability = 0.2f;

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

        var levelIndex = PlayerPrefs.GetInt(PlayerResources.LevelIndexKey, 0);

        switch (levelIndex)
        {
            case 0:
                GenerateRegular();
                break;

            case 1:
                GenerateRegular();
                break;

            case 2:
                GenerateMarketplace();
                break;

            case 3:
                // One-eye Spider boss
                GenerateBoss(0);
                break;

            case 4:
                GenerateRegular();
                break;

            case 5:
                GenerateRegular();
                break;

            case 6:
                GenerateMarketplace();
                break;

            case 7:
                // Shielded Fly boss
                GenerateBoss(1);
                break;

            case 8:
                GenerateRegular();
                break;

            case 9:
                GenerateRegular();
                break;

            case 10:
                GenerateRegular();
                break;

            case 11:
                GenerateMarketplace();
                break;

            case 12:
                // TODO: Third boss fight
                GenerateBoss(2);
                break;

            case 13:
                GenerateRegular();
                break;

            case 14:
                GenerateRegular();
                break;

            case 15:
                GenerateRegular();
                break;

            case 16:
                GenerateMarketplace();
                break;

            case 17:
                // TODO: Fourth boss fight
                GenerateBoss(3);
                break;

            case 18:
                GenerateMarketplace();
                break;

            case 19:
                // TODO: Fifth boss fight
                GenerateBoss(4);
                break;

            default:
                // TODO: Game completed animation then go back to home
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

    private void GenerateRegular()
    {
        Variant = LevelVariant.Regular;

        while (_currentBlock > MinBlock)
        {
            _currentBlock -= SpawnPlatform().size.y;
            SpawnFloatingEnemy();
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
}
