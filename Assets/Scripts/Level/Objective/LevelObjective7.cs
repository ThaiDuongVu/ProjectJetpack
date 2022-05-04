using UnityEngine;

public class LevelObjective7 : LevelObjective
{
    private float groundTimer = 0f;

    public override bool IsCompleted
    {
        get => groundTimer <= 8f;
        set => base.IsCompleted = value;
    }

    #region Unity Event

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Player.IsGrounded && !Player.basePlatformReached) groundTimer += Time.fixedDeltaTime;
    }

    #endregion
}
