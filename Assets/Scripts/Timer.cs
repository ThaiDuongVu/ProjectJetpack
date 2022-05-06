using UnityEngine;

public class Timer
{
    private float _timerMax;
    private float _progress;

    public Timer(float timerMax)
    {
        _timerMax = timerMax;
        _progress = 0f;
    }

    public void Reset(float max)
    {
        _timerMax = max;
        _progress = 0f;
    }

    public bool IsReached()
    {
        _progress += Time.fixedDeltaTime;
        return _progress >= _timerMax;
    }
}