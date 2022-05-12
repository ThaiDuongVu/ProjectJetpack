using System.Collections;
using UnityEngine;

public class RocketSewerCombat : EnemyCombat
{
    private RocketSewer _rocketSewer;

    [SerializeField] private Rocket rocketPrefab;
    [SerializeField] private ParticleSystem fireMuzzle;
    [SerializeField] private int fireFrame;
    [SerializeField] private Transform firePoint;

    private static readonly int AttackAnimationTrigger = Animator.StringToHash("attack");

    #region Unity Event

    public override void Awake()
    {
        _rocketSewer = GetComponent<RocketSewer>();
    }

    #endregion
    
    public IEnumerator Fire(Transform target)
    {
        _rocketSewer.Animator.SetTrigger(AttackAnimationTrigger);

        yield return new WaitForSeconds(fireFrame / 60f);

        var firePointPosition = firePoint.position;
        Instantiate(rocketPrefab, firePointPosition, Quaternion.identity).Target = target;
        Instantiate(fireMuzzle, firePointPosition, Quaternion.identity).transform.up = -Vector2.up;
    }
}
