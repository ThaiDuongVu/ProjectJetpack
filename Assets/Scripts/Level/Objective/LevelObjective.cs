using UnityEngine;

public class LevelObjective : MonoBehaviour
{
    private bool _isCompleted;
    public virtual bool IsCompleted
    {
        get => _isCompleted;
        set
        {
            _isCompleted = value;
        }
    }

    public new string name;

    public Player Player { get; set; }

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
