using UnityEngine;

public class Timer
{
    private float _timerMax;
    public float Progress;

    public Timer(float timerMax)
    {
        _timerMax = timerMax;
        Progress = 0f;
    }

    public void Reset(float max)
    {
        _timerMax = max;
        Progress = 0f;
    }

    public bool IsReached()
    {
        Progress += Time.fixedDeltaTime;
        return Progress >= _timerMax;
    }
}