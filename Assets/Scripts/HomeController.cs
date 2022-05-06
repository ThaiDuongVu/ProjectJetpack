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
        Invoke(nameof(LoadTokens), 0.1f);
    }

    #endregion

    private void LoadTokens()
    {
        _playerPreview.PlayerResources.Token = PlayerPrefs.GetInt(Player.TokensKey, 0);
    }
}