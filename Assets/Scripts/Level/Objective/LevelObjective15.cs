using UnityEngine;

public class LevelObjective15 : LevelObjective
{
    private bool _comboReached;

    public override bool IsCompleted
    {
        get => _comboReached;
        set => base.IsCompleted = value;
    }

    #region Unity Event

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Player.PlayerCombo.Multiplier >= 5 && !_comboReached) _comboReached = true;
    }

    #endregion
}
