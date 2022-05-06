using UnityEngine;

public class BeamTurtleCombat : EnemyCombat
{
    [SerializeField] private Transform beam;
    [SerializeField] private float range = 5f;
    [SerializeField] private int damage = 2;
    [SerializeField] private float knockBackForce = 15f;
    [SerializeField] private Transform bulletEndPoint;
    [SerializeField] private BulletEffect bulletEffectPrefab;

    private Player _target;
    private float _fireTimer;
    private const float FireTimerMax = 1f;

    #region Unity Event

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Aim();
        if (_fireTimer > 0f) _fireTimer -= Time.fixedDeltaTime;
    }

    #endregion

    private void Aim()
    {
        _target = null;

        var hit = Physics2D.Raycast(beam.position, Vector2.up, range);
        if (!hit) return;

        if (!hit.transform.CompareTag("Player")) return;
        
        _target = hit.transform.GetComponent<Player>();
        Invoke(nameof(Fire), Random.Range(0.2f, 0.4f));
    }

    public void Fire()
    {
        if (_fireTimer > 0f) return;

        if (_target)
        {
            _target.TakeDamage(damage);
            _target.KnockBack(Vector2.up, knockBackForce);
        }

        Instantiate(bulletEffectPrefab, beam.position, Quaternion.identity).TargetPosition = bulletEndPoint.position;
        _fireTimer = FireTimerMax;
    }
}
