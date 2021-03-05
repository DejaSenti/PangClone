using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public UnityEvent TimerElapsedEvent;

    public int RoundTimeLeft { get => (int)(duration - elapsed) + 1; }

    private float duration;
    private float elapsed;

    private void Awake()
    {
        ResetTimer();
    }

    public void StartTimer(float duration)
    {
        this.duration = duration;

        elapsed = 0;
        enabled = true;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > duration)
        {
            ResetTimer();
            if (TimerElapsedEvent != null)
                TimerElapsedEvent.Invoke();
        }
    }

    public void ResetTimer()
    {
        enabled = false;
        duration = 0;
        elapsed = 0;
    }
}