using UnityEngine;

public class Bomb : Enemy
{
    public float xPositionStart = -4.5f;
    [SerializeField] private bool isStatic;

    #region Unity Event

    public override void Start()
    {
        base.Start();

        if (!isStatic)
        {
            var xPositionLeftDistance = Random.Range(0, 10);
            var bombTransform = transform;
            bombTransform.position = new Vector2(xPositionStart + xPositionLeftDistance, bombTransform.position.y);
        }
    }

    #endregion

    public override void Die()
    {
        base.Die();

        AudioController.Instance.Play(AudioVariant.Explode1);
    }
}
