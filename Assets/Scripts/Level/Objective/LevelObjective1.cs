using UnityEngine;

public class LevelObjective1 : LevelObjective
{
    private int _initHealth;

    public override bool IsCompleted
    {
        get => _initHealth - Player.PlayerResources.Health <= 0;
        protected set => base.IsCompleted = value;
    }

    #region Unity Event

    public override void Start()
    {
        base.Start();

        _initHealth = PlayerPrefs.GetInt(PlayerResources.HealthTempKey, Player.PlayerResources.maxHealth);
    }

    #endregion
}
