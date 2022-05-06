public class LevelObjective13 : LevelObjective
{
    public override bool IsCompleted
    {
        get => FindObjectsOfType<Enemy>().Length <= 0;
        protected set => base.IsCompleted = value;
    }
}
