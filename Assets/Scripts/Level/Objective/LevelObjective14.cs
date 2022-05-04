using UnityEngine;

public class LevelObjective14 : LevelObjective
{
    private int initEnemiesCount;

    public override bool IsCompleted
    {
        get => FindObjectsOfType<Enemy>().Length <= (initEnemiesCount / 2);
        set => base.IsCompleted = value;
    }

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        initEnemiesCount = FindObjectsOfType<Enemy>().Length;
    }

    #endregion
}
