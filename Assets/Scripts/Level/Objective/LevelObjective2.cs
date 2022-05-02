using UnityEngine;

public class LevelObjective2 : LevelObjective
{
    private int _initHealth;

    public override bool IsCompleted
    {
        get => _initHealth - Player.PlayerResources.Health <= 1;
        set => base.IsCompleted = value;
    }

    #region Unity Event

    public override void Start()
    {
        base.Start();

        _initHealth = Player.PlayerResources.Health;
    }

    #endregion
}
