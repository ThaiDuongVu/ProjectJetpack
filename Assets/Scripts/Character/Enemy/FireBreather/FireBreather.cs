using System.Collections;
using UnityEngine;

public class FireBreather : Enemy
{
    [SerializeField] private Vector2 fireDelayRange = new Vector2(1f, 2f);
    [SerializeField] private Vector2 fireIntervalRange = new Vector2(3f, 5f);

    private bool _firstFired;
    public FireBreatherCombat FireBreatherCombat { get; private set; }

    private Transform _playerTransform;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _playerTransform = FindObjectOfType<Player>().transform;
        FireBreatherCombat = GetComponent<FireBreatherCombat>();
    }

    public override void Start()
    {
        base.Start();

        StartCoroutine(FireRepeat());
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        SetFlipped(_playerTransform.position.x < transform.position.x);
    }

    #endregion

    private IEnumerator FireRepeat()
    {
        if (_firstFired) yield return new WaitForSeconds(Random.Range(fireIntervalRange.x, fireIntervalRange.y));
        else yield return new WaitForSeconds(Random.Range(fireDelayRange.x, fireDelayRange.y));

        _firstFired = true;

        FireBreatherCombat.Fire();
        StartCoroutine(FireRepeat());
    }

    public override void Die()
    {
        StopAllCoroutines();

        base.Die();
    }
}
