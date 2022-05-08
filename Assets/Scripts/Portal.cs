using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{
    public float xPositionStart = -3.5f;
    [SerializeField] private bool randomizePosition = true;

    private Transform _target;
    private bool _targetEntered;
    private const float InterpolationRatio = 0.05f;
    private const float EnterDelay = 1f;
    [SerializeField] private Transform enterPoint;
    [SerializeField] private UnityEvent onEntered;

    private InputPrompt _inputPrompt;

    #region Unity Event

    private void Awake()
    {
        _inputPrompt = GetComponentInChildren<InputPrompt>();
    }

    private void Start()
    {
        if (randomizePosition)
        {
            var xPositionLeftDistance = Random.Range(0, 6);
            var portalTransform = transform;
            portalTransform.position = new Vector2(xPositionStart + xPositionLeftDistance, portalTransform.position.y);
        }

        _inputPrompt.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!_targetEntered) return;

        _target.position = Vector2.Lerp(_target.position, enterPoint.position, InterpolationRatio);
        _target.localScale = Vector2.Lerp(_target.localScale, Vector2.zero, InterpolationRatio);
    }

    #endregion

    public IEnumerator Enter(Player player)
    {
        _target = player.transform;
        player.IsControllable = false;
        player.Animator.SetBool(CharacterMovement.IsRunningAnimationTrigger, true);
        _inputPrompt.gameObject.SetActive(false);
        _targetEntered = true;

        yield return new WaitForSeconds(EnterDelay);

        onEntered.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _inputPrompt.gameObject.SetActive(true);

        var player = other.GetComponent<Player>();
        player.Portal = this;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _inputPrompt.gameObject.SetActive(false);

        var player = other.GetComponent<Player>();
        player.Portal = null;
    }
}
