public class DemonicRatPathfinder : CharacterPathfinder
{
    private DemonicRat _demonicRat;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _demonicRat = GetComponent<DemonicRat>();
    }

    #endregion

    public override void OnPathReached()
    {
        base.OnPathReached();
        StopTracking();
        if (_demonicRat.State == DemonicRatState.Wander) _demonicRat.StartWandering();
    }
}