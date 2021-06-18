using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible

    #region Singleton

    private static EnemySpawner instance;

    public static EnemySpawner Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<EnemySpawner>();

            return instance;
        }
    }

    #endregion

    public int MaxPopulation { get; set; } = 5;
    public int CurrentPopulation { get; set; }
    [SerializeField] private Enemy[] enemyPrefabs;

    [SerializeField] private Vector2 minPosition;
    [SerializeField] private Vector2 maxPosition;

    public bool IsInWave { get; set; }

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {

    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {

    }

    /// <summary>
    /// Unity Event function.
    /// Update once per frame.
    /// </summary>
    private void Update()
    {

    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {

    }

    /// <summary>
    /// Spawn an enemy in the game world.
    /// </summary>
    public void Spawn()
    {
        Enemy enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(enemyToSpawn, new Vector2(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y)), transform.rotation);
        CurrentPopulation++;
    }
}
