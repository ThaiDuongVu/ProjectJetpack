using UnityEngine;

public class Trail : MonoBehaviour
{
    public Transform Target { get; set; }
    private readonly Vector2 offset = new Vector2(0f, -0.5f);
    private const float InterpolationRatio = 0.5f;

    private ParticleSystem.MainModule main;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        main = GetComponent<ParticleSystem>().main;
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        if (!Target) return;
        transform.position = Vector3.Lerp(transform.position, (Vector2)Target.transform.position + offset, InterpolationRatio);
    }

    /// <summary>
    /// Set current particle color.
    /// </summary>
    /// <param name="color">Color to set</param>
    public void SetColor(Color color)
    {
        main.startColor = color;
    }
}
