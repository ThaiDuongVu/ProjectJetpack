using UnityEngine;

public class MovingPlatform : Platform
{
    [SerializeField] private int[] xMinPositions;
    [SerializeField] private int[] xMaxPositions;
    [SerializeField] private Vector2 velocityRange = new(2f, 4f);

    private Rigidbody2D _rigidbody2D;
    private int _xMinPosition;
    private int _xMaxPosition;
    private float _velocity;
    private Vector2 _direction;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public override void Start()
    {
        base.Start();

        _xMinPosition = xMinPositions[Random.Range(0, xMinPositions.Length)];
        _xMaxPosition = xMaxPositions[Random.Range(0, xMaxPositions.Length)];

        _velocity = Random.Range(velocityRange.x, velocityRange.y);
        _direction = new Vector2(Random.Range(-1f, 1f), 0f).normalized;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if ((transform.position.x <= _xMinPosition && _direction == Vector2.left)
        || (transform.position.x >= _xMaxPosition && _direction == Vector2.right))
            _direction = -_direction;

        _rigidbody2D.MovePosition(_rigidbody2D.position + _direction * (_velocity * Time.fixedDeltaTime));
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.GetComponent<Character>()) return;

        other.transform.parent = transform;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (!other.transform.GetComponent<Character>()) return;

        other.transform.parent = null;
    }
}
