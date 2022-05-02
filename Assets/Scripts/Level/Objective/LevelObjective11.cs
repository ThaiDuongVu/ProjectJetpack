using UnityEngine;

public class LevelObjective11 : LevelObjective
{
    private float timer = 0f;

    public override bool IsCompleted
    {
        get => timer <= 90f;
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
