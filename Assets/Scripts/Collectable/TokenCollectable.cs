using UnityEngine;

public class TokenCollectable : Collectable
{
    protected override void OnCollected()
    {
        base.OnCollected();
        target.Resources.Tokens++;
    }
}
