using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible

    #region Singleton

    private static MainCameraController _mainCameraControllerInstance;

    public static MainCameraController Instance
    {
        get
        {
            if (_mainCameraControllerInstance == null) _mainCameraControllerInstance = FindObjectOfType<MainCameraController>();
            return _mainCameraControllerInstance;
        }
    }

    #endregion

    public Transform target;

    private const float YOffset = -2f;
    private const float FollowInterpolationRatio = 0.1f;
    private const float MinYPosition = -80f;
    private const float MaxYPosition = 0f;

    #region Unity Event

    private void FixedUpdate()
    {
        if (!target) return;

        Follow();
    }

    #endregion

    private void Follow()
    {
        var targetPosition = target.position;
        var lerpPosition = new Vector3(0f, Mathf.Clamp(targetPosition.y + YOffset, MinYPosition, MaxYPosition), -10f);

        transform.position = Vector3.Lerp(transform.position, lerpPosition, FollowInterpolationRatio);
    }
}
