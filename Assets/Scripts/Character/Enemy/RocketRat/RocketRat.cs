using UnityEngine;

public class RocketRat : Enemy
{
    [SerializeField] private GameObject rocketSewersPrefab;

    private Portal _portal;

    #region Unity Event

    public override void Awake()
    {
        base.Awake();

        _portal = FindObjectOfType<Portal>();
    }

    public override void Start()
    {
        base.Start();

        _portal.gameObject.SetActive(false);
        Instantiate(rocketSewersPrefab, transform.position, Quaternion.identity);
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
}
