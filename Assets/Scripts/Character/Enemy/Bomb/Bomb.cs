using UnityEngine;

public class Bomb : Enemy
{
    public float xPositionStart = -4.5f;

    #region Unity Event

    public override void Start()
    {
        base.Start();

        var xPositionLeftDistance = Random.Range(0, 10);
        transform.position = new Vector2(xPositionStart + xPositionLeftDistance, transform.position.y);
    }

    #endregion
}
