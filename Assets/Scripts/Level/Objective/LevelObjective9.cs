using UnityEngine;

public class LevelObjective9 : LevelObjective
{
    private float timer = 0f;

    public override bool IsCompleted
    {
        get => timer <= 60f;
        set => base.IsCompleted = value;
    }

    #region Unity Event

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!Player.BasePlatformReached) timer += Time.fixedDeltaTime;
    }

    #endregion
}
