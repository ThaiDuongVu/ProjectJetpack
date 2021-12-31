using UnityEngine;
using TMPro;

public class PlayerCombo : MonoBehaviour
{
    public int Multiplier { get; private set; }
    private float timer;
    private const float TimerMax = 5f;

    [SerializeField] private TMP_Text text;
    private RectTransform textTransform;
    private const float TextScaleFactor = 1f;
    private const float TextScaleInterpolationRatio = 0.2f;

    /// <summary>
    /// Unity Event function.
    /// Get component references.
    /// </summary>
    private void Awake()
    {
        textTransform = text.GetComponent<RectTransform>();
        timer = 0f;
        textTransform.localScale = Vector2.zero;
    }

    /// <summary>
    /// Unity Event function.
    /// Update once per frame.
    /// </summary>
    private void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;

        if (timer > 0f) timer -= Time.fixedDeltaTime * Time.timeScale;
        else Cancel();

        textTransform.localScale = Vector2.Lerp(textTransform.localScale, new Vector2(1f, 1f) * (timer / TimerMax * TextScaleFactor), TextScaleInterpolationRatio);
    }

    /// <summary>
    /// Add combo multiplier.
    /// </summary>
    /// <param name="amount">Amount to multiply</param>
    public void Add(int amount)
    {
        Multiplier += amount;
        timer = TimerMax;

        textTransform.localRotation = new Quaternion(0f, 0f, Random.Range(-0.2f, 0.2f), 1f);
        text.text = "x" + Multiplier;
    }

    /// <summary>
    /// Cancel current combo.
    /// </summary>
    public void Cancel()
    {
        Multiplier = 1;
        timer = 0f;
    }
}