using UnityEngine;

public class CharacterResources : MonoBehaviour
{
    private Character _character;

    [Header("Health")]
    public int maxHealth = 9;
    private int _health;
    public virtual int Health
    {
        get => _health;
        set
        {
            _health = value;
            if (value <= 0) _character.Die();
        }
    }

    #region Unity Event

    public virtual void Awake()
    {
        _character = GetComponent<Character>();
    }

    public virtual void Start()
    {
        Health = maxHealth;
    }

    #endregion
}