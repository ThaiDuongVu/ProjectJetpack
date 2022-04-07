using UnityEngine;

public class Nest : Enemy
{
    [SerializeField] private Transform eye;
    private Transform _playerTransform;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _playerTransform = FindObjectOfType<Player>().transform;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        LookAt(_playerTransform);
    }

    #endregion

    private void LookAt(Transform target)
    {
        eye.up = (target.position - transform.position).normalized;
    }
}
