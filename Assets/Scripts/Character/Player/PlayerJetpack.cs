using UnityEngine;

public class PlayerJetpack : MonoBehaviour
{
    public Player Player { get; set; }

    [Header("Jump Properties")]
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private Transform jumpPoint;
    [SerializeField] private ParticleSystem jumpMuzzlePrefab;
    [SerializeField] private int jumpRate = 4;
    private Timer _jumpTimer;
    private float _jumpTimeout;
    private bool _canJump = true;

    [Header("Hover Properties")]
    [SerializeField] private float hoverForce = 26f;
    [SerializeField] private ParticleSystem hoverMuzzle;
    public bool IsHovering { get; set; }

    [Header("Combat Properties")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float range = 8f;
    [SerializeField] private ParticleSystem bloodSplashPrefab;

    [Header("Crosshair Properties")]
    public SpriteRenderer crosshair;
    [SerializeField] private Color regularColor;
    [SerializeField] private Color aimColor;

    [Header("Fuel Properties")]
    public float fuelConsumptionPerJump = 0.1f;
    [SerializeField] private float fuelConsumptionPerHoverSecond = 0.25f;
    [SerializeField] private float fuelRechargeRate = 0.5f;

    private Enemy _targetEnemy;
    private Vector2 _hitPoint;
    [SerializeField] private BulletEffect bulletEffectPrefab;

    private DestructablePlatform _targetDestructablePlatform;

    #region Unity Event

    private void Start()
    {
        _jumpTimeout = 1f / jumpRate;
        _jumpTimer = new Timer(_jumpTimeout);
    }

    private void FixedUpdate()
    {
        Aim();

        if (IsHovering) Hover();
        if (_jumpTimer.IsReached()) _canJump = true;
        if (Player.IsGrounded) Player.PlayerResources.Fuel += fuelRechargeRate * Time.fixedDeltaTime;
    }

    #endregion

    private void Aim()
    {
        _targetEnemy = null;
        _targetDestructablePlatform = null;
        crosshair.color = regularColor;

        var hit = Physics2D.Raycast(transform.position, Vector2.down, range);
        if (!hit) return;

        var isHit = false;

        if (hit.transform.CompareTag("Enemy"))
        {
            _targetEnemy = hit.transform.GetComponent<Enemy>();
            isHit = true;
        }
        else if (hit.transform.CompareTag("DestructablePlatform"))
        {
            _targetDestructablePlatform = hit.transform.GetComponent<DestructablePlatform>();
            isHit = true;
        }

        if (!isHit) return;

        _hitPoint = hit.point;
        crosshair.color = aimColor;
    }

    public void Jump()
    {
        if (!_canJump || Player.PlayerResources.Fuel < fuelConsumptionPerJump) return;

        Player.Rigidbody2D.velocity = Vector2.zero;
        Player.Rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        Player.PlayerResources.Fuel -= fuelConsumptionPerJump;

        Instantiate(jumpMuzzlePrefab, jumpPoint.position, Quaternion.identity);

        _canJump = false;
        _jumpTimer.Reset(_jumpTimeout);

        Fire();
    }

    #region Hover Methods

    public void SetHovering(bool value)
    {
        if (Player.PlayerResources.Fuel <= 0f) return;

        Player.Rigidbody2D.velocity = Vector2.zero;

        if (value)
        {
            hoverMuzzle.Play();
            AudioController.Instance.Play(AudioVariant.Hover);
        }
        else
        {
            hoverMuzzle.Stop();
            AudioController.Instance.Stop(AudioVariant.Hover);
        }

        IsHovering = value;
    }

    public void StopHovering()
    {
        SetHovering(false);
    }

    private void Hover()
    {
        if (Player.PlayerResources.Fuel <= 0f)
        {
            SetHovering(false);
            return;
        }

        Player.Rigidbody2D.AddForce(Vector2.up * hoverForce, ForceMode2D.Force);
        Player.PlayerResources.Fuel -= fuelConsumptionPerHoverSecond * Time.fixedDeltaTime;

        CameraShaker.Instance.Shake(CameraShakeMode.Nano);
    }

    #endregion

    private void Fire()
    {
        if (_targetEnemy)
        {
            _targetEnemy.TakeDamage(damage);
            Instantiate(bloodSplashPrefab, _hitPoint, Quaternion.identity);
            Instantiate(bulletEffectPrefab, transform.position, Quaternion.identity).TargetPosition = _hitPoint;

            CameraShaker.Instance.Shake(CameraShakeMode.Normal);
            AudioController.Instance.Play(AudioVariant.JumpDamage);
            Player.PlayerCombo.Add();
        }
        else if (_targetDestructablePlatform)
        {
            _targetDestructablePlatform.Explode();
            Instantiate(bulletEffectPrefab, transform.position, Quaternion.identity).TargetPosition = _hitPoint;

            CameraShaker.Instance.Shake(CameraShakeMode.Normal);
            AudioController.Instance.Play(AudioVariant.JumpDamage);
        }
        else
        {
            CameraShaker.Instance.Shake(CameraShakeMode.Light);
            AudioController.Instance.Play(AudioVariant.JumpNoDamage);
        }
    }
}
