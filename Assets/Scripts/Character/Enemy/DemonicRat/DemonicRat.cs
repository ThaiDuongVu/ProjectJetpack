using UnityEngine;

public class DemonicRat : Enemy
{
    public DemonicRatMovement DemonicRatMovement { get; private set; }
    public DemonicRatCombat DemonicRatCombat { get; private set; }
    public DemonicRatResources DemonicRatResources { get; private set; }

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        DemonicRatMovement = GetComponent<DemonicRatMovement>();
        DemonicRatCombat = GetComponent<DemonicRatCombat>();
        DemonicRatResources = GetComponent<DemonicRatResources>();
    }

    #endregion
}
