using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Fireball : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    public FireBreather FireBreather { get; set; }

    [Header("Sprite and Color")]
    [SerializeField] private SpriteRenderer mainSprite;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Light2D light2d;
    [SerializeField] private Color friendlyColor;

    [Header("Velocity Properties")]
    [SerializeField] private float trackingVelocity = 5f;
    [SerializeField] private float returnVelocity = 20f;


    [Header("Damage Properties")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float staggerForce = 10f;

    [SerializeField] private ParticleSystem explosionRedPrefab;
    [SerializeField] private string friendlyLayer;

    public Transform Target { get; set; }
    public Transform Sender { get; set; }
    private Vector2 _currentDirection;
    private float _currentVelocity;

    private CollectibleSpawner[] _collectibleSpawners;

    #region Unity Event

    private void Awake()
    {
        Target = FindObjectOfType<Player>().transform;

        _rigidbody2d = GetComponent<Rigidbody2D>();
        _collectibleSpawners = GetComponentsInChildren<CollectibleSpawner>();
    }

    private void Start()
    {
        _currentVelocity = trackingVelocity;
    }

    private void FixedUpdate()
    {
        if (!Target)
        {
            Instantiate(explosionRedPrefab, transform.position, Quaternion.identity);
            FireBreather.BulletsCount--;
            Destroy(gameObject);
            return;
        }

        _currentDirection = ((Target.position + Vector3.up * 0.5f) - transform.position).normalized;
        _rigidbody2d.MovePosition(_rigidbody2d.position + _currentDirection * (_currentVelocity * Time.fixedDeltaTime));
    }

    #endregion

    public void ReturnToSender()
    {
        mainSprite.color = friendlyColor;
        light2d.color = friendlyColor;
        trail.startColor = friendlyColor;

        Target = Sender;
        _currentVelocity = returnVelocity;

        foreach (var spawner in _collectibleSpawners) spawner.Spawn();
        gameObject.layer = LayerMask.NameToLayer(friendlyLayer);

        Instantiate(explosionRedPrefab, transform.position, Quaternion.identity);
        GameController.Instance.StartCoroutine(GameController.Instance.SlowDownEffect());
    }

    public void Explode()
    {
        Instantiate(explosionRedPrefab, transform.position, Quaternion.identity);
        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
        FireBreather.BulletsCount--;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var character = other.transform.GetComponent<Character>();
        if (character)
        {
            character.TakeDamage(damage);
            character.Stagger(_currentDirection, staggerForce);
        }

        Explode();
    }
}