using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    private InputPrompt _inputPrompt;
    private CollectibleSpawner[] _collectibleSpawners;
    [SerializeField] private int price;

    #region Unity Event

    private void Awake()
    {
        _inputPrompt = GetComponentInChildren<InputPrompt>();
        _collectibleSpawners = GetComponentsInChildren<CollectibleSpawner>();
    }

    private void Start()
    {
        _inputPrompt.gameObject.SetActive(false);
    }

    #endregion

    public void Purchase(Player player)
    {
        if (player.PlayerResources.Token < price) 
        {
            GameController.Instance.SendUIMessage("Not enough tokens!");
            return;
        }

        foreach (var collectible in _collectibleSpawners) collectible.Spawn();
        player.PlayerResources.Token -= price;

        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _inputPrompt.gameObject.SetActive(true);

        var player = other.GetComponent<Player>();
        player.VendingMachine = this;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _inputPrompt.gameObject.SetActive(false);

        var player = other.GetComponent<Player>();
        player.VendingMachine = null;
    }
}
