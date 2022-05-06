public class LevelObjective17 : LevelObjective
{
    private bool _comboReached;

    public override bool IsCompleted
    {
        get => _comboReached;
        protected set => base.IsCompleted = value;
    }

    #region Unity Event

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Player.PlayerCombo.Multiplier >= 10 && !_comboReached) _comboReached = true;
    }

    #endregion
}
