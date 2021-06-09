using System;
using UnityEngine;

public class TimeToAttack : MonoBehaviour
{
    public float MinAttackRate { get; set; }
    public float MaxAttackRate { get; set; }
    private float timer;
    private float timerMax;

    public Action OnTimerReset { get; set; }

    /// <summary>
    /// Reset attack timer.
    /// </summary>
    public void ResetTimer()
    {
        timer = 0f;
        timerMax = UnityEngine.Random.Range(MinAttackRate, MaxAttackRate);
        OnTimerReset.Invoke();
    }

    /// <summary>
    /// Check if attack timer is reached.
    /// </summary>
    /// <return>Whether attack timer is reached</return>
    public bool IsTimer()
    {
        timer += Time.fixedDeltaTime;
        return timer >= timerMax;
    }
}
