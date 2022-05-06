public class LevelObjective14 : LevelObjective
{
    private int _initEnemiesCount;

    public override bool IsCompleted
    {
        get => FindObjectsOfType<Enemy>().Length <= (_initEnemiesCount / 2);
        protected set => base.IsCompleted = value;
    }

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _initEnemiesCount = FindObjectsOfType<Enemy>().Length;
    }

    #endregion
}
