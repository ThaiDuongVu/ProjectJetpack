using UnityEngine;

public class LevelObjective : MonoBehaviour
{
    public virtual bool IsCompleted { get; protected set; }

    public new string name;

    protected Player Player { get; private set; }

    #region Unity Event

    public virtual void Awake()
    {
        Player = FindObjectOfType<Player>();
    }

    public virtual void Start()
    {
    }

    public virtual void FixedUpdate()
    {
    }

    #endregion
}
