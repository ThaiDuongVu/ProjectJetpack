using UnityEngine;

public class LevelObjective13 : LevelObjective
{
    public override bool IsCompleted
    {
        get => FindObjectsOfType<Enemy>().Length <= 0;
        set => base.IsCompleted = value;
    }
}
