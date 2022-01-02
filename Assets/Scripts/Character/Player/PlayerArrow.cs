using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    public Player Player { get; private set; }

    public Vector2 Direction { get; set; } = Vector2.up;
    private const float InterpolationRatio = 0.5f;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        Player = GetComponent<Player>();
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        transform.up = Vector2.Lerp(transform.up, Direction, InterpolationRatio);
    }
}
