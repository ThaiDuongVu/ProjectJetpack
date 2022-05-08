using UnityEngine;

public class FlyShieldSet : MonoBehaviour
{
    public ShieldedFly Target { get; set; }
    private FlyShield[] _flyShields;

    private const float RotateSpeed = 1f;

    #region Unity Event

    private void Awake()
    {
        _flyShields = GetComponentsInChildren<FlyShield>();
    }

    private void FixedUpdate()
    {
        transform.position = Target.transform.position;
        transform.RotateAround(transform.position, Vector3.forward, -RotateSpeed);
    }

    #endregion

    public void Die()
    {
        foreach (var shield in _flyShields) shield.Die();
        // Destroy(gameObject);
    }
}
