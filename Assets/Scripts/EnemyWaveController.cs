using UnityEngine;

public class EnemyWaveController : MonoBehaviour
{
    [Header("Wave Properties")]
    [SerializeField]
    private float waveDuration; // How long should a wave of enemies be
    [SerializeField] private float waveInterval; // How long should waves be between one another
    [SerializeField] private float maxWaveCount = Mathf.Infinity;
    public int CurrentWaveCount { get; set; } = 0;
    public bool IsInWave { get; set; }
    public bool IsWaveCompleted { get; set; }

    [SerializeField] private Vector2 regularSpawnRateRange = new Vector2(0.5f, 1.5f);
    private Timer _regularWaveTimer;

    [SerializeField] private Vector2 midSpawnRateRange = new Vector2(10f, 12f);
    private Timer _midWaveTimer;

    #region Unity Event

    private void Start()
    {
        _regularWaveTimer = new Timer(Random.Range(regularSpawnRateRange.x, regularSpawnRateRange.y));
        _midWaveTimer = new Timer(Random.Range(midSpawnRateRange.x, midSpawnRateRange.y));
        Invoke(nameof(StartWave), 0f);
    }

    private void FixedUpdate()
    {
        if (GameController.Instance.State != GameState.InProgress) return;
        if (!IsInWave) return;

        if (_regularWaveTimer.IsReached())
        {
            EnemySpawner.Instance.SpawnRegular();
            _regularWaveTimer.Reset(Random.Range(regularSpawnRateRange.x, regularSpawnRateRange.y));
        }

        if (_midWaveTimer.IsReached())
        {
            EnemySpawner.Instance.SpawnMid();
            _midWaveTimer.Reset(Random.Range(midSpawnRateRange.x, midSpawnRateRange.y));
        }
    }

    #endregion

    #region Wave Control Methods

    private void StartWave()
    {
        if (CurrentWaveCount >= maxWaveCount)
        {
            StopWavePermanent();
            IsWaveCompleted = true;
            return;
        }

        CurrentWaveCount++;
        IsInWave = true;
        Invoke(nameof(StopWave), waveDuration);
    }

    private void StopWave()
    {
        IsInWave = false;
        Invoke(nameof(StartWave), waveInterval);
    }

    public void StopWavePermanent()
    {
        IsInWave = false;
        CancelInvoke();
    }

    #endregion
}