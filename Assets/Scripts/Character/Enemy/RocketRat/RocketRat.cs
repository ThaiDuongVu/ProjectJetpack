using UnityEngine;
using System.Collections;

public class RocketRat : Enemy
{
    [SerializeField] private GameObject rocketSewersPrefab;

    private Portal _portal;

    private static readonly int AttackAnimationTrigger = Animator.StringToHash("attack");
    private int _sewerCount;
    public int SewerCount
    {
        get => _sewerCount;
        set
        {
            _sewerCount = value;
            if (value <= 0) Die();
        }
    }

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _portal = FindObjectOfType<Portal>();
    }

    private new IEnumerator Start()
    {
        _portal.gameObject.SetActive(false);
        var sewers = Instantiate(rocketSewersPrefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.1f);

        SewerCount = sewers.GetComponentsInChildren<RocketSewer>().Length;
    }

    #endregion

    public override void TakeDamage(int damage)
    {
        // base.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();

        _portal.gameObject.SetActive(true);
        AudioController.Instance.Play(AudioVariant.Explode2);
        AudioController.Instance.Play(AudioVariant.PlayerReachBasePlatform);
    }

    public void Attack()
    {
        Animator.SetTrigger(AttackAnimationTrigger);
    }
}
