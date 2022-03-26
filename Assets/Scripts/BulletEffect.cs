using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    private const float InterpolationRatio = 0.4f;
    private const float Epsilon = 0.1f;

    public Vector2 targetPosition;

    #region Unity Event

    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, targetPosition, InterpolationRatio);
        if (Vector2.Distance(transform.position, targetPosition) <= Epsilon) Destroy(gameObject);
    }

    #endregion
}
