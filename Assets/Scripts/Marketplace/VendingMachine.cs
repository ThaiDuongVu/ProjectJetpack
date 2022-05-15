using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    private InputPrompt _inputPrompt;
    protected CollectibleSpawner[] CollectibleSpawners;
    public int price;

    #region Unity Event

    public virtual void Awake()
    {
        _inputPrompt = GetComponentInChildren<InputPrompt>();
        CollectibleSpawners = GetComponentsInChildren<CollectibleSpawner>();
    }

    public virtual void Start()
    {
        _inputPrompt.gameObject.SetActive(false);
    }

    #endregion

    public virtual void Purchase(Player player)
    {
        if (player.PlayerResources.Token < price)
        {
            GameController.Instance.SendUIMessage("Not enough tokens!");
            return;
        }

        foreach (var collectible in CollectibleSpawners) collectible.Spawn();
        player.PlayerResources.Token -= price;

        GameController.Instance.StartCoroutine(GameController.Instance.SlowMotionEffect());
        AudioController.Instance.Play(AudioVariant.UnlockCustomization);
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
