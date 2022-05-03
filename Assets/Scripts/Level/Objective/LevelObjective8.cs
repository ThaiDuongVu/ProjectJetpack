using UnityEngine;

public class LevelObjective8 : LevelObjective
{
    private float groundTimer = 0f;

    public override bool IsCompleted
    {
        get => groundTimer <= 10f;
        set => base.IsCompleted = value;
    }

    #region Unity Event

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Player.IsGrounded && !Player.BasePlatformReached) groundTimer += Time.fixedDeltaTime;
    }

    #endregion
}
