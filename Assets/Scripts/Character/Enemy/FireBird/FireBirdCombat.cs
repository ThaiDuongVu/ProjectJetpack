using System.Collections;
using UnityEngine;

public class FireBirdCombat : EnemyCombat
{
    private FireBird _fireBird;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ParticleSystem fireMuzzle;
    [SerializeField] private int fireFrame;
    private static readonly int AttackAnimationTrigger = Animator.StringToHash("attack");

    [SerializeField] private Fireball fireballPrefab;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _fireBird = GetComponent<FireBird>();
    }

    #endregion

    public IEnumerator Fire(Transform target)
    {
        _fireBird.Animator.SetTrigger(AttackAnimationTrigger);

        yield return new WaitForSeconds(fireFrame / 60f);

        Instantiate(fireballPrefab, firePoint.position, Quaternion.identity).Target = target;
        Instantiate(fireMuzzle, firePoint.position, Quaternion.identity).transform.up = _fireBird.IsFlipped ? Vector2.right : Vector2.left;
    }
}
