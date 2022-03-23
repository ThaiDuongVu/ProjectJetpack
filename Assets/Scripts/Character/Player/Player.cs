using UnityEngine;

public class Player : Character
{
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerCombat PlayerCombat { get; private set; }
    public PlayerResources PlayerResources { get; private set; }
    public PlayerCombo PlayerCombo { get; private set; }

    [SerializeField] private Trail groundTrailPrefab;
    public Trail GroundTrail { get; private set; }

    [SerializeField] private ParticleSystem whiteExplosionPrefab;

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

    #endregion

    #region Damage & Death

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        PlayerCombo.Cancel();
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
}