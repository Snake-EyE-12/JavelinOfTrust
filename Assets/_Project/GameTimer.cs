using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float timeOfStart;
    private bool isStarted;
    private void StartTime()
    {
        if (!isStarted)
        {
            isStarted = true;
            timeOfStart = Time.time;
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            StartTime();
        }
    }

    private void OnEnable()
    {
        Target.container.OnUpdate += OnTargetUpdated;
    }
    private void OnDisable()
    {
        Target.container.OnUpdate -= OnTargetUpdated;
    }

    private void OnTargetUpdated(int count, float accuracy)
    {
        if (count >= 10)
        {
            EndTime();
        }
    }

    private bool stopped;
    private void EndTime()
    {
        finalTime = GetTime();
        stopped = true;
        OnEndTimer?.Invoke();
    }

    public Action OnEndTimer = delegate { };

    private float finalTime;

    public float GetTime() => stopped || !isStarted ? finalTime : Time.time - timeOfStart;
    
}