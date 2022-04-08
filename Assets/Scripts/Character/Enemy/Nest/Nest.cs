using UnityEngine;

public class Nest : Enemy
{
    [SerializeField] private Transform eye;
    private Transform _playerTransform;

    [SerializeField] private EnemyWaveController postDeathEnemyWaveController;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _playerTransform = FindObjectOfType<Player>().transform;
        postDeathEnemyWaveController.enabled = false;
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

    public override void Die()
    {
        postDeathEnemyWaveController.transform.parent = null;
        postDeathEnemyWaveController.enabled = true;
        GameController.Instance.SendUIMessage("Nest destroyed, here they comes!");

        base.Die();
    }
}
