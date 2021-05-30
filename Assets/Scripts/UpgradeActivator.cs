using UnityEngine;
using TMPro;

public class UpgradeActivator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private int upgradeId;
    private PlayerUpgrade upgradeRef;

    private new Camera camera;
    private const float InterpolationRatio = 0.2f;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        upgradeRef = Player.Instance.upgrades[upgradeId];
        camera = Camera.main;
    }

    /// <summary>
    /// Unity Event function.
    /// Initialize before first frame update.
    /// </summary>
    private void Start()
    {
        spriteRenderer.sprite = upgradeRef.icon;
        nameText.text = upgradeRef.name;
        descriptionText.text = upgradeRef.description;
        costText.text = "Cost - " + upgradeRef.cost.ToString();
    }

    /// <summary>
    /// Unity Event function.
    /// Update at consistent time.
    /// </summary>
    private void FixedUpdate()
    {
        transform.up = Vector2.Lerp(transform.up, camera.transform.up, InterpolationRatio);
    }

    /// <summary>
    /// Activate the desired upgrade.
    /// </summary>
    public void Activate()
    {
        if (Player.Instance.Resources.CollectedCoins > upgradeRef.cost)
        {
            upgradeRef.isActive = true;
            Player.Instance.Resources.CollectedCoins -= upgradeRef.cost;

            // Upgrade flavours
            Instantiate(Player.Instance.dashSlicePrefab, Player.Instance.transform.position, Player.Instance.transform.rotation);
            Instantiate(explosion, transform.position, transform.rotation);
            GameController.Instance.StartCoroutine(GameController.Instance.SlowDownEffect());

            // Destroy activator
            Destroy(gameObject);
        }
        else
        {
            // UIController.Instance.SendMessage("Not enough coins to upgrade!");
        }

        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
    }
}
