using UnityEngine;
using Pathfinding;

public class CharacterPathfinder : MonoBehaviour
{
    private Character _character;
    public bool IsTracking { get; set; }

    private Path _currentPath;
    private Seeker _seeker;

    [SerializeField]
    private float nextWaypointDistance = 0.2f;
    private int _currentWaypoint;

    #region Unity Event

    public virtual void Awake()
    {
        _character = GetComponent<Character>();
        _seeker = GetComponent<Seeker>();
    }
    
    public virtual void FixedUpdate()
    {
        if (_currentPath == null || !IsTracking) return;

        if (_currentWaypoint >= _currentPath.vectorPath.Count)
        {
            OnPathReached();
            return;
        }

        var direction = (_currentPath.vectorPath[_currentWaypoint] - transform.position).normalized;
        _character.CharacterMovement.StartRunning(direction);

        if (Vector2.Distance(transform.position, _currentPath.vectorPath[_currentWaypoint]) <= nextWaypointDistance)
            _currentWaypoint++;
    }
    
    #endregion
    
    public void FindPath(Transform target)
    {
        _seeker.StartPath(transform.position, target.transform.position, path =>
        {
            if (path.error) return;

            _currentPath = path;
            _currentWaypoint = 0;
        });
    }
    
    public void FindPath(Vector2 position)
    {
        _seeker.StartPath(transform.position, position, path =>
        {
            if (path.error) return;

            _currentPath = path;
            _currentWaypoint = 0;
        });
    }
    
    #region Tracking Methods
    
    public void StartTracking()
    {
        IsTracking = true;
    }
    
    public void StopTracking()
    {
        IsTracking = false;
        _character.CharacterMovement.StopRunning();
    }
    
    #endregion
    
    public virtual void OnPathReached()
    {
    }
}