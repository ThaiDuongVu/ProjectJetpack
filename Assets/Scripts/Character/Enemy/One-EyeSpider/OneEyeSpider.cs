using UnityEngine;

public class OneEyeSpider : Enemy
{
    private OneEyeSpiderState _state;
    private OneEyeSpiderMovement _oneEyeSpiderMovement;

    [SerializeField] private Vector2 wanderDurationRange = new Vector2(2f, 4f);
    [SerializeField] private Vector2 idleDurationRange = new Vector2(1f, 2f);
    public Vector2 Direction { get; private set; }

    [SerializeField] private SpiderEye eyePrefab;

    private Portal _portal;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _oneEyeSpiderMovement = GetComponent<OneEyeSpiderMovement>();
        _portal = FindObjectOfType<Portal>();
    }

    public override void Start()
    {
        base.Start();

        _portal.gameObject.SetActive(false);

        Direction = new Vector2(Random.Range(-1f, 1f), 0f).normalized;
        Invoke(nameof(StartWandering), Random.Range(idleDurationRange.x, idleDurationRange.y));

        Instantiate(eyePrefab, transform.position, Quaternion.identity).OneEyeSpider = this;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsEdged && _state == OneEyeSpiderState.Wander)
        {
            CancelInvoke();
            StopWandering();
        }
    }

    #endregion

    public override void Die()
    {
        base.Die();

        _portal.gameObject.SetActive(true);
        AudioController.Instance.Play(AudioVariant.Explode2);
    }

    private void StartWandering()
    {
        Direction = -Direction;
        SetFlipped(!IsFlipped);
        _oneEyeSpiderMovement.StartRunning(Direction);
        _state = OneEyeSpiderState.Wander;

        Invoke(nameof(StopWandering), Random.Range(wanderDurationRange.x, wanderDurationRange.y));
    }

    private void StopWandering()
    {
        _oneEyeSpiderMovement.StopRunningImmediate();
        _state = OneEyeSpiderState.Idle;

        Invoke(nameof(StartWandering), Random.Range(idleDurationRange.x, idleDurationRange.y));
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        CancelInvoke();
        StopWandering();
    }
}
