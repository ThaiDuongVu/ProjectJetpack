using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerCombat PlayerCombat { get; private set; }
    public PlayerResources PlayerResources { get; private set; }
    public PlayerCombo PlayerCombo { get; private set; }
    public PlayerArrow PlayerArrow { get; private set; }

    [SerializeField] private Trail groundTrailPrefab;
    public Trail GroundTrail { get; private set; }

    [SerializeField] private ParticleSystem whiteExplosionPrefab;

    private Portal nearbyPortal;
    private List<string> _collectKeyIds = new List<string>();

    private InputManager _inputManager;

    #region Input Methods

    private void InteractOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State == GameState.Paused) return;
        InputTypeController.Instance.CheckInputType(context);

        if (nearbyPortal)
        {
            if (nearbyPortal.IsOpen) nearbyPortal.onEntered.Invoke();
            else nearbyPortal.Unlock(_collectKeyIds);
        }
    }

    #endregion

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle interact input
        _inputManager.Player.Interact.performed += InteractOnPerformed;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }
    public override void Awake()
    {
        base.Awake();

        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerCombat = GetComponent<PlayerCombat>();
        PlayerResources = GetComponent<PlayerResources>();
        PlayerCombo = GetComponent<PlayerCombo>();
        PlayerArrow = GetComponentInChildren<PlayerArrow>();
    }

    public override void Start()
    {
        base.Start();

        GroundTrail = Instantiate(groundTrailPrefab, transform.position, Quaternion.identity);
        GroundTrail.Target = transform;
    }

    #endregion

    public override void Stagger(Vector2 direction)
    {
        PlayerCombat.SetDash(false);
        base.Stagger(direction);
    }

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

    public void CollectKey(string keyId)
    {
        _collectKeyIds.Add(keyId);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal")) nearbyPortal = other.GetComponent<Portal>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Portal")) nearbyPortal = null;
    }
}