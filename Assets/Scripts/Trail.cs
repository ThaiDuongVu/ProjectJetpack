using UnityEngine;

public class Trail : MonoBehaviour
{
    public Transform Target { get; set; }
    [SerializeField] private Material particleMaterial;
    private ParticleSystem particle;
    private ParticleSystem.MainModule mainModule;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        mainModule = particle.main;
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        if (!Target) return;

        // Set trail position to target position
        transform.position = new Vector3(Target.position.x, transform.position.y, Target.position.z);
    }

    /// <summary>
    /// Set current particle color.
    /// </summary>
    /// <param name="color">Color to set</param>
    public void SetColor(Color color)
    {
        // mainModule.startColor = color;
        particleMaterial.color = color;
    }
}