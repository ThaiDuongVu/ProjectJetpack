using System.Collections;
using UnityEngine;

public class LevelObjective20 : LevelObjective
{
    private int _initEnemiesCount;

    public override bool IsCompleted
    {
        get => FindObjectsOfType<Enemy>().Length - _initEnemiesCount >= 5;
        protected set => base.IsCompleted = value;
    }

    #region Unity Event

    private new IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        _initEnemiesCount = FindObjectsOfType<Enemy>().Length;
    }

    #endregion
}
