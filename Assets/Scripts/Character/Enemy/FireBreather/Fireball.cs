using UnityEngine;

public class Fireball : Enemy
{
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    private const float ColliderDelay = 0.2f;
    [SerializeField] private int damage = 1;

    [Header("Velocity")]
    private float _currentVelocity;
    private Vector2 _currentDirection;
    [SerializeField] private float trackingVelocity = 5f;
    [SerializeField] private float returnVelocity = 20f;

    [Header("Colors and Effects")]
    [SerializeField] private Color blue;
    [SerializeField] private Color red;
    [SerializeField] private TrailRenderer trail;

    public Transform Target { get; set; }
    public Transform Sender { get; set; }

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _collider2D.enabled = false;
    }

    public override void Start()
    {
        base.Start();

        _currentVelocity = trackingVelocity;
        Invoke(nameof(EnableCollider), ColliderDelay);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!Target) Die();
        
        _currentDirection = (Target.position - transform.position).normalized;
        _rigidbody2D.MovePosition(_rigidbody2D.position + _currentDirection * _currentVelocity * Time.fixedDeltaTime);
    }

    #endregion

    private void EnableCollider()
    {
        _collider2D.enabled = true;
    }

    public override void TakeDamage(int damage)
    {
        ReturnToSender();
    }

    public override void Die()
    {
        CameraShaker.Instance.Shake(CameraShakeMode.Light);

        base.Die();
    }

    public void ReturnToSender()
    {
        if (!Sender) Die();

        Target = Sender;
        _currentVelocity = returnVelocity;

        mainSprite.color = blue;
        trail.startColor = trail.endColor = blue;

        _collider2D.enabled = false;
        Invoke(nameof(EnableCollider), ColliderDelay);
        GameController.Instance.StartCoroutine(GameController.Instance.SlowDownEffect());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var character = other.transform.GetComponent<Character>();
        if (character && !character.transform.CompareTag("Fireball"))
        {
            character.TakeDamage(damage);
            character.Stagger(_currentDirection);
        }

        Die();
    }
}
