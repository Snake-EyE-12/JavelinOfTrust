using System;
using NaughtyAttributes;
using UnityEngine;

public class RollOutBanner : MonoBehaviour
{
    
    [SerializeField] private float finalScale = 0.7f;
    [SerializeField] private float rollTime = 1f;
    [SerializeField] private AnimationCurve rollCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private Transform scaleTransform;
    [SerializeField] private GameObject birdBannerPart;
    private bool rolling;
    private float timeOfRollStart;

    private float initialScaleY;

    [Button]
    public void StartRollOut()
    {
        initialScaleY = scaleTransform.localScale.y;
        rolling = true;
        timeOfRollStart = Time.time;
        //birdBannerPart.SetActive(birdBanner);
    }

    public void SetAsBirdBanner()
    {
        birdBannerPart.SetActive(true);
    }

    public void StartRollOut(float time) => Invoke(nameof(StartRollOut), time);

    private void Update()
    {
        
        if (!rolling) return;

        float timeSinceStart = Time.time - timeOfRollStart;

        if (timeSinceStart > rollTime)
        {
            rolling = false;
            timeSinceStart = rollTime;
        }
        
        float t = timeSinceStart / rollTime;
        float easedT = rollCurve.Evaluate(t);
        SetScale(easedT);
    }

    private void SetScale(float t)
    {
        scaleTransform.localScale = new Vector3(scaleTransform.localScale.x, CalculateYScale(t), scaleTransform.localScale.z);
    }

    private float CalculateYScale(float t)
    {
        return Mathf.Lerp(initialScaleY, finalScale, t);
    }

    // private void Update()
    // {
    //     if (!rolling) return;
    //
    //     float timeSinceStart = Time.time - timeOfRollStart;
    //
    //     if (timeSinceStart > rollTime)
    //     {
    //         rolling = false;
    //         timeSinceStart = rollTime; // Clamp
    //     }
    //
    //     float t = timeSinceStart / rollTime;
    //     float easedT = rollCurve.Evaluate(t);
    //
    //     // Assume scale.y = 1 corresponds to object "unit height"
    //     // So desired final scale is simply 'length' in world units
    //     float currentScaleY = Mathf.Lerp(0f, length, easedT);
    //
    //     // Apply new scale
    //     transform.localScale = new Vector3(initialScale.x, currentScaleY, initialScale.z);
    // }


}
