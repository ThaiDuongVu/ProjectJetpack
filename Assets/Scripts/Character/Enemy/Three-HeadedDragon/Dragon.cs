using UnityEngine;

public class Dragon : Enemy
{
    private Portal _portal;

    private int _dragonHeadsCount;
    public int DragonHeadsCount
    {
        get => _dragonHeadsCount;
        set
        {
            _dragonHeadsCount = value;
            if (value <= 0) Die();
        }
    }

    private Player _target;
    [SerializeField] private float detectRadius = 18f;
    [SerializeField] private Vector2 fireIntervalRange = new Vector2(1f, 3f);

    private DragonCombat _dragonCombat;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _portal = FindObjectOfType<Portal>();
        DragonHeadsCount = GetComponentsInChildren<DragonHead>().Length;
        _dragonCombat = GetComponent<DragonCombat>();
    }

    public override void Start()
    {
        base.Start();

        _portal.gameObject.SetActive(false);
        Invoke(nameof(Attack), Random.Range(fireIntervalRange.x, fireIntervalRange.y));
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        DetectTarget();
    }

    #endregion

    public override void Die()
    {
        base.Die();

        _portal.gameObject.SetActive(true);
        AudioController.Instance.Play(AudioVariant.PlayerReachBasePlatform);
    }

    private void DetectTarget()
    {
        if (_target) return;

        var hits = Physics2D.OverlapCircleAll(transform.position, detectRadius);

        foreach (var hit in hits)
            if (hit.transform.CompareTag("Player"))
                _target = hit.GetComponent<Player>();
    }

    private void Attack()
    {
        if (_target) _dragonCombat.Attack();
        Invoke(nameof(Attack), Random.Range(fireIntervalRange.x, fireIntervalRange.y));
    }
}
