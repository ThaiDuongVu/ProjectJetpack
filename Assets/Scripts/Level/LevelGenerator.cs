using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Vector2[] levelPositions;
    [SerializeField] private LevelFragment[] levelFragmentPrefabs;

    #region Unity Event

    private void Start()
    {
        Generate();
    }

    #endregion

    private void Generate()
    {
        foreach (var position in levelPositions)
        {
            Instantiate(levelFragmentPrefabs[Random.Range(0, levelFragmentPrefabs.Length)], position, Quaternion.identity).transform.parent = transform;
        }
    }
}
