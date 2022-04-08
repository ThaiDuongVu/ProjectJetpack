using UnityEngine;

public class Key : Collectible
{
    public string[] ids;

    #region Unity Event

    public override void Start()
    {
        InitPosition = transform.position;
        EnableCollect();
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
