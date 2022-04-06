using UnityEngine;

public class FireBreatherCombat : EnemyCombat
{
    [SerializeField] private Transform gunPoint;
    [SerializeField] private Fireball fireballPrefab;
    [SerializeField] private ParticleSystem muzzlePrefab;

    private Player _player;
    private FireBreather _fireBreather;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _player = FindObjectOfType<Player>();
        _fireBreather = GetComponent<FireBreather>();
    }

    #endregion

    public void Fire()
    {
        if (!_player) return;
        
        var fireball = Instantiate(fireballPrefab, gunPoint.position, Quaternion.identity);
        fireball.Sender = transform;
        fireball.Target = _player.transform;

        Instantiate(muzzlePrefab, gunPoint.position, Quaternion.identity).transform.up = _fireBreather.IsFlipped ? Vector2.right : Vector2.left;
    }
}
