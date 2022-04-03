using UnityEngine;

public class Player : Character
{
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerCombat PlayerCombat { get; private set; }
    public PlayerResources PlayerResources { get; private set; }
    public PlayerCombo PlayerCombo { get; private set; }

    [SerializeField] private Trail groundTrailPrefab;
    public Trail GroundTrail { get; private set; }
    [SerializeField] private Color transparent;
    [SerializeField] private Color white;

    [SerializeField] private ParticleSystem whiteExplosionPrefab;

    public bool IsAirbourne { get; set; }
    [SerializeField] private float airbourneVelocityThreshold = 0.1f;
    private static readonly int IsAirbourneAnimationTrigger = Animator.StringToHash("isAirbourne");

    [SerializeField] private SpriteRenderer arrow;
    private const float ArrowPosition = 7.5f;
    private const float MaxPositionY = 9f;

    public bool CanExit { get; set; }

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerCombat = GetComponent<PlayerCombat>();
        PlayerResources = GetComponent<PlayerResources>();
        PlayerCombo = GetComponent<PlayerCombo>();
    }

    public override void Start()
    {
        base.Start();

        GroundTrail = Instantiate(groundTrailPrefab, transform.position, Quaternion.identity);
        GroundTrail.Target = transform;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        DetectAirbourne();

        if (transform.position.y >= MaxPositionY)
        {
            arrow.gameObject.SetActive(true);
            arrow.transform.position = new Vector3(transform.position.x, 7.5f);
        }
        else arrow.gameObject.SetActive(false);
    }

    #endregion

    private void DetectAirbourne()
    {
        IsAirbourne = Mathf.Abs(Rigidbody2D.velocity.y) > airbourneVelocityThreshold;
        Animator.SetBool(IsAirbourneAnimationTrigger, IsAirbourne);
        GroundTrail.SetColor(IsAirbourne ? transparent : white);
    }

    #region Damage & Death

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        PlayerCombo.Cancel();
        PlayerCombat.ExitHoverMode();
    }

    public override void Die()
    {
        base.Die();

        Instantiate(whiteExplosionPrefab, transform.position, Quaternion.identity);
        GameController.Instance.StartCoroutine(GameController.Instance.GameOver());

        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
        GameController.Instance.StartCoroutine(GameController.Instance.SlowDownEffect());

        Destroy(gameObject);
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BottomBorder")) Die();
        else if (other.CompareTag("Portal"))
        {
            var portal = other.GetComponent<Portal>();
            if (portal.IsOpen) CanExit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Portal")) CanExit = false;
    }
}