using UnityEngine;

public class FireBird : Enemy
{
    private Player _target;
    [SerializeField] private float detectRadius = 8f;
    [SerializeField] private Vector2 fireIntervalRange = new Vector2(1f, 3f);

    private FireBirdCombat _fireBirdCombat;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _fireBirdCombat = GetComponent<FireBirdCombat>();
    }

    public override void Start()
    {
        base.Start();

        Invoke(nameof(Fire), Random.Range(fireIntervalRange.x, fireIntervalRange.y));
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        DetectTarget();

        if (_target) SetFlipped(_target.transform.position.x < transform.position.x);
    }

    #endregion

    private void DetectTarget()
    {
        _target = null;

        var hits = Physics2D.OverlapCircleAll(transform.position, detectRadius);

        foreach (var hit in hits)
            if (hit.transform.CompareTag("Player"))
                _target = hit.GetComponent<Player>();
    }

    private void Fire()
    {
        if (_target) StartCoroutine(_fireBirdCombat.Fire(_target.transform));

        Invoke(nameof(Fire), Random.Range(fireIntervalRange.x, fireIntervalRange.y));
    }
}
