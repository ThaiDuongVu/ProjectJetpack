using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private AstarPath pathfinder;

    #region Unity Event

    private void Awake()
    {
        pathfinder = GetComponent<AstarPath>();
    }

    private void Start()
    {
        Generate();
        pathfinder.Scan();
    }

    #endregion

    private void Generate()
    {

    }
}