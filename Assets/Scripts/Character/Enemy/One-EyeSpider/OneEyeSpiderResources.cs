using UnityEngine;

public class OneEyeSpiderResources : EnemyResources
{
    [SerializeField] private Transform healthDisplay;
    private SpriteRenderer[] _healthIcons;

    public override int Health
    {
        get => base.Health;
        set
        {
            base.Health = value;
            UpdateHealthDisplay();
        }
    }

    #region Unity Engine

    public override void Awake()
    {
        base.Awake();

        _healthIcons = healthDisplay.GetComponentsInChildren<SpriteRenderer>();
    }

    #endregion

    private void UpdateHealthDisplay()
    {
        if (Health < 0) return;
        for (var i = 0; i < Health; i++) _healthIcons[i].gameObject.SetActive(true);
        for (var i = Health; i < maxHealth; i++) _healthIcons[i].gameObject.SetActive(false);
    }
}
