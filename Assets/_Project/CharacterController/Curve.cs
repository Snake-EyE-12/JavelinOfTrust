using System;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public class Curve
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private Vector2 xRange;
    [SerializeField] private Vector2 yRange;
    
    public bool WithinRange(float x) => x >= xRange.x && x <= xRange.y;
    
    public float Evaluate(float x) => curve.Evaluate((Mathf.Clamp(x, xRange.x, xRange.y) - xRange.x) / (xRange.y - xRange.x)) * (yRange.y - yRange.x) + yRange.x;
    
    public bool TryEvaluate(float x, out float result)
    {
        result = 0;
        if (!WithinRange(x)) return false;
        result = Evaluate(x);
        return true;
    }
}