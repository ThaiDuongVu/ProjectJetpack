using UnityEngine;

public class Key : Collectible
{
    [SerializeField] private string[] ids;
    [SerializeField] private bool disableOnStartup;

    #region Unity Event

    public override void Start()
    {
        InitPosition = transform.position;
        EnableCollect();

        gameObject.SetActive(!disableOnStartup);
    }

    #endregion

    public override void OnCollected(Transform target)
    {
        if (IsCollected || !CanBeCollected) return;

        var player = target.GetComponent<Player>();
        foreach (var id in ids) player.CollectKey(id);
        GameController.Instance.SendUIMessage("Key collected");
        
        base.OnCollected(target);
    }
}
