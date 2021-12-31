using UnityEngine;

public class HomeController : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible

    #region Singleton

    private static HomeController homeControllerInstance;

    public static HomeController Instance
    {
        get
        {
            if (homeControllerInstance == null) homeControllerInstance = FindObjectOfType<HomeController>();
            return homeControllerInstance;
        }
    }

    #endregion

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        EffectsController.Instance.SetDepthOfField(true);
        EffectsController.Instance.SetChromaticAberration(false);
        EffectsController.Instance.SetVignetteIntensity(EffectsController.DefaultVignetteIntensity);
    }
}
