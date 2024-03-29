using UnityEngine;

public class LevelObjective7 : LevelObjective
{
    private float _groundTimer;

    public override bool IsCompleted
    {
        get => _groundTimer <= 8f;
        protected set => base.IsCompleted = value;
    }

    #region Unity Event

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Player.IsGrounded && !Player.basePlatformReached) _groundTimer += Time.fixedDeltaTime;
    }

    #endregion
}
