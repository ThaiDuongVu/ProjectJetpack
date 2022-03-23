using UnityEngine;

public class Trail : MonoBehaviour
{
    public Transform Target { get; set; }
    private readonly Vector2 _offset = new Vector2(0f, -0.55f);
    private const float InterpolationRatio = 0.5f;

    private ParticleSystem.MainModule _main;

    #region Unity Event

    private void Awake()
    {
        _main = GetComponent<ParticleSystem>().main;
    }

    private void FixedUpdate()
    {
        if (!Target) return;
        transform.position = Vector3.Lerp(transform.position, (Vector2) Target.transform.position + _offset,
            InterpolationRatio);
    }

    #endregion

    public void SetColor(Color color)
    {
        _main.startColor = color;
    }
}