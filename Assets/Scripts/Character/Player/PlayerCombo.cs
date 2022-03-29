using UnityEngine;
using TMPro;

public class PlayerCombo : MonoBehaviour
{
    public int Multiplier { get; private set; }
    private float _timer;
    private const float TimerMax = 6f;

    [SerializeField] private TMP_Text text;
    private RectTransform _textTransform;
    private const float TextScaleFactor = 1f;
    private const float TextScaleInterpolationRatio = 0.2f;

    #region Unity Event

    private void Awake()
    {
        _textTransform = text.GetComponent<RectTransform>();
        _timer = 0f;
        _textTransform.localScale = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;

        if (_timer > 0f) _timer -= Time.fixedDeltaTime * Time.timeScale;
        else Cancel();

        _textTransform.localScale = Vector2.Lerp(_textTransform.localScale,
            Vector2.one * (_timer / TimerMax * TextScaleFactor), TextScaleInterpolationRatio);
    }

    #endregion

    public void Add(int amount)
    {
        Multiplier += amount;
        _timer = TimerMax;

        _textTransform.localRotation = new Quaternion(0f, 0f, Random.Range(-0.2f, 0.2f), 1f);
        text.text = "x" + Multiplier;
    }

    public void Cancel()
    {
        Multiplier = 0;
        _timer = 0f;
    }
}