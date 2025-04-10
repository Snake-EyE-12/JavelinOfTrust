using System;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public class Curve
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private Vector2 xRange;
    [SerializeField] private Vector2 yRange;
    
    public float Evaluate(float x) => curve.Evaluate((Mathf.Clamp(x, xRange.x, xRange.y) - xRange.x) / (xRange.y - xRange.x)) * (yRange.y - yRange.x) + yRange.x;
}