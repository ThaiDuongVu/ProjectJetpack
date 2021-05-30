using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private Animator cameraAnimator;
    private static readonly int OutroTrigger = Animator.StringToHash("outro");
    [SerializeField] private AnimationClip cameraOutroAnimationClip;

    private string sceneToLoad = "";

    private Canvas[] canvases;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        if (Camera.main is { }) cameraAnimator = Camera.main.GetComponent<Animator>();

        canvases = FindObjectsOfType<Canvas>();
    }

    /// <summary>
    /// Load a scene in the background.
    /// </summary>
    private IEnumerator Load()
    {
        // Load scene in background but don't allow transition
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

        // Enable motion blur effect
        EffectsController.Instance.SetMotionBlur(true);
        EffectsController.Instance.SetDepthOfField(false);
        // Play camera animation
        cameraAnimator.SetTrigger(OutroTrigger);

        // Disable all game UI
        foreach (Canvas canvas in canvases)
            canvas.gameObject.SetActive(false);

        // Wait for camera animation to complete
        yield return new WaitForSeconds(cameraOutroAnimationClip.averageDuration);

        // Allow transition to new scene
        asyncOperation.allowSceneActivation = true;
    }

    /// <summary>
    /// Load a scene.
    /// </summary>
    /// <param name="scene">Scene to load</param>
    public void Load(string scene)
    {
        Time.timeScale = 1f;
        sceneToLoad = scene;

        StartCoroutine(Load());
    }

    /// <summary>
    /// Restart scene.
    /// </summary>
    public void Restart()
    {
        // Reload current active scene
        Load(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Quit game.
    /// </summary>
    public static void Quit()
    {
        Application.Quit();
    }
}