using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible

    #region Singleton

    private static MainCameraController mainCameraControllerInstance;

    public static MainCameraController Instance
    {
        get
        {
            if (mainCameraControllerInstance == null) mainCameraControllerInstance = FindObjectOfType<MainCameraController>();
            return mainCameraControllerInstance;
        }
    }

    #endregion

    public Animator Animator { get; private set; }

    public Transform FollowTarget { get; set; }
    private const float MaxFollowTime = 1f / 6f;
    private float currentFollowTime;
    private bool isInFollowMode;
    public bool IsInFollowMode
    {
        get => isInFollowMode;
        set
        {
            isInFollowMode = value;
            currentFollowTime = 0f;
        }
    }

    private static readonly int IsZoomedInAnimationTrigger = Animator.StringToHash("isZoomedIn");

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Unity Event function.
    /// Update once per frame.
    /// </summary>
    private void Update()
    {
        if (IsInFollowMode) Follow();
        else Revert();
    }

    /// <summary>
    /// Set whether current camera is zoomed in or not.
    /// </summary>
    /// <param name="zoom">Value to set</param>
    public void SetZoom(bool zoom)
    {
        Animator.SetBool(IsZoomedInAnimationTrigger, zoom);
    }

    /// <summary>
    /// Follow current target around in game world.
    /// </summary>
    private void Follow()
    {
        if (!FollowTarget) return;
        if (currentFollowTime > MaxFollowTime) return;

        currentFollowTime += Time.deltaTime;
        transform.localPosition = Vector3.Lerp(transform.position, FollowTarget.position, currentFollowTime / MaxFollowTime);
    }

    /// <summary>
    /// Revert back to default position.
    /// </summary>
    private void Revert()
    {
        if (currentFollowTime > MaxFollowTime) return;

        currentFollowTime += Time.deltaTime;
        transform.localPosition = Vector3.Lerp(transform.position, Vector3.zero, currentFollowTime / MaxFollowTime);
    }
}
