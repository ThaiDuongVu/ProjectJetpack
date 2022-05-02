using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    private const float InterpolationRatio = 0.4f;
    private const float Epsilon = 0.1f;
    public Vector2 TargetPosition { get; set; }

    #region Unity Event

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, TargetPosition) <= Epsilon)
        {
            Destroy(gameObject);
            return;
        }
        
        transform.position = Vector2.Lerp(transform.position, TargetPosition, InterpolationRatio);
    }

    #endregion
}
