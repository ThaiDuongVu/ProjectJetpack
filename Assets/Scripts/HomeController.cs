using System.Collections;
using UnityEngine;

public class HomeController : MonoBehaviour
{
    private Player _playerPreview;

    #region Unity Event

    private void Awake()
    {
        _playerPreview = FindObjectOfType<Player>();
    }

    private void Start()
    {
        _playerPreview.PlayerResources.ClearTemp();
        // StartCoroutine(PlayHomeIntroAnimation());
    }

    #endregion

    private IEnumerator PlayHomeIntroAnimation()
    {
        _playerPreview.PlayerMovement.StartRunning(Vector2.right);

        yield return new WaitForSeconds(0.75f);

        _playerPreview.PlayerMovement.StopRunning();

        yield return new WaitForSeconds(2f);

        _playerPreview.PlayerMovement.StartRunning(Vector2.right);

        yield return new WaitForSeconds(0.5f);

        _playerPreview.PlayerMovement.StopRunning();

        yield return new WaitForSeconds(0.25f);

        _playerPreview.EnterPortal();
    }
}