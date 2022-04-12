using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    private const float InterpolationRatio = 0.6f;
    public Vector2 TargetDirection { get; set; } = Vector2.up;
    public Vector2 CurrentDirection => transform.up;

    [SerializeField] private SpriteRenderer sprite;

    #region Unity Event

    private void FixedUpdate()
    {
        transform.up = Vector2.Lerp(transform.up, TargetDirection, InterpolationRatio);
    }

    #endregion

    public void SetColor(Color color)
    {
        sprite.color = color;
    }
}