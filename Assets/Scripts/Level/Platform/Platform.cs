using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector2Int size;
    public float[] xPositions;

    public float spawnEnemyProbability;
    public int spawnEnemyCount;

    #region Unity Event

    public virtual void Awake()
    {
    }

    public virtual void Start()
    {
    }

    public virtual void FixedUpdate()
    {
    }

    #endregion
}
