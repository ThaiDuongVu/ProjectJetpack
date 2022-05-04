using UnityEngine;

public class LevelObjective6 : LevelObjective
{
    private float groundTimer = 0f;

    public override bool IsCompleted
    {
        get => groundTimer <= 5f;
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
