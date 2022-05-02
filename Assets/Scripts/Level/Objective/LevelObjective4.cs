using UnityEngine;

public class LevelObjective4 : LevelObjective
{
    private int _initHealth;

    public override bool IsCompleted
    {
        get => _initHealth - Player.PlayerResources.Health <= 3;
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