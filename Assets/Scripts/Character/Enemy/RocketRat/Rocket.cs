using UnityEngine;

public class Rocket : Fireball
{
    #region Unity Event

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        transform.up = direction;
    }

    #endregion
}
