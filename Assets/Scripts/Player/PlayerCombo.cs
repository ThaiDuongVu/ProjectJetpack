using UnityEngine;
using TMPro;

public class PlayerCombo : MonoBehaviour
{
    public int Multiplier { get; set; }
    private float timer;
    private const float TimerMax = 5f;

    [SerializeField] private TMP_Text text;
    [SerializeField] private RectTransform textTransform;
    private const float TextScaleFactor = 1.5f;

    /// <summary>
    /// Unity Event function.
    /// Update once per frame.
    /// </summary>
    private void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;

        if (timer > 0f) timer -= Time.fixedDeltaTime * Time.timeScale;
        else if (timer <= 0f) Cancel();

        textTransform.localScale = new Vector2(1f, 1f) * (timer / TimerMax * TextScaleFactor);
    }

    /// <summary>
    /// Add combo multiplier.
    /// </summary>
    /// <param name="amount">Amount to multiply</param>
    public void Add(int amount)
    {
        Multiplier += amount;
        timer = TimerMax;

        textTransform.localRotation = new Quaternion(0f, 0f, Random.Range(-0.25f, 0.25f), 1f);
        text.text = "x" + Multiplier.ToString();
    }

    /// <summary>
    /// Cancel current combo.
    /// </summary>
    public void Cancel()
    {
        Multiplier = 0;
        timer = 0f;
    }
}