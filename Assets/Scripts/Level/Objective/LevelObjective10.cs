using UnityEngine;

public class LevelObjective10 : LevelObjective
{
    private float _timer;

    public override bool IsCompleted
    {
        get => _timer <= 45f;
        protected set => base.IsCompleted = value;
    }

    #region Unity Event

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!Player.basePlatformReached) _timer += Time.fixedDeltaTime;
    }

    #endregion
}
