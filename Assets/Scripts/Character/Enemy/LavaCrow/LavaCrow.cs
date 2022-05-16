using UnityEngine;

public class LavaCrow : Enemy
{
    private float _velocity = 1f;
    private float _acceleration = 0.2f;

    private Player _player;
    private Lava[] _lavas;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _player = FindObjectOfType<Player>();
        _lavas = GetComponentsInChildren<Lava>();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!_player) return;

        Rigidbody2D.MovePosition(Rigidbody2D.position + Vector2.down * _velocity * Time.fixedDeltaTime);
        _velocity += Time.fixedDeltaTime * _acceleration;

        if (_player.basePlatformReached)
        {
            foreach (var lava in _lavas) lava.Die();
            Die();
        }
    }

    #endregion

    public override void TakeDamage(int damage)
    {
        // base.TakeDamage(damage);
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        // base.OnCollisionEnter2D(other);
    }
}
