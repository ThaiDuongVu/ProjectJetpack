using UnityEngine;

public class RocketSewer : Enemy
{
    private RocketSewerCombat _rocketSewerCombat;

    private Player _target;
    [SerializeField] private float detectRadius = 8f;
    [SerializeField] private Vector2 fireIntervalRange = new Vector2(1f, 3f);

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _rocketSewerCombat = GetComponent<RocketSewerCombat>();
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
    }

    #endregion

    public override void Die()
    {
        base.Die();

        AudioController.Instance.Play(AudioVariant.Explode2);
    }

    private void DetectTarget()
    {
        if (_target) return;

        var hits = Physics2D.OverlapCircleAll(transform.position, detectRadius);

        foreach (var hit in hits)
            if (hit.transform.CompareTag("Player"))
                _target = hit.GetComponent<Player>();
    }

    private void Fire()
    {
        if (_target) StartCoroutine(_rocketSewerCombat.Fire(_target.transform));

        Invoke(nameof(Fire), Random.Range(fireIntervalRange.x, fireIntervalRange.y));
    }
}
