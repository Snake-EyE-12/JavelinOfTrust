using System;
using NaughtyAttributes;
using UnityEngine;

public class Javelin : MonoBehaviour
{
    private float power;
    private Vector2 ownerVel;
    private Vector2 dir;
    public bool inAir;
    private float startTime;
    public void Throw(float timeCharged, Vector3 ownerVelocity, Transform owner, Vector2 direction)
    {
        power = CalculatePower(timeCharged);
        
        ownerVel = ownerVelocity;
        dir = direction;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        transform.position = owner.position;
        inAir = true;
        startTime = Time.time;
    }

    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float fallOffVelocityTime;
    [SerializeField] private float inheritedPercent;
    private void Update()
    {
        if (!inAir) return;
        float normalizedTimeHeld = Mathf.Clamp01((Time.time - startTime) / fallOffVelocityTime);
        Debug.Log("percent of inherited: " + ((1 - normalizedTimeHeld) * inheritedPercent));
        body.linearVelocity = (dir * power) + ((1 - normalizedTimeHeld) * inheritedPercent * ownerVel);
    }

    [SerializeField] private Transform collisionPoint;
    public void HitWall(Vector2 contact)
    {
        inAir = false;
        body.linearVelocity = Vector2.zero;
        transform.position += ((Vector3)contact - collisionPoint.position);
    }

    [SerializeField] private AnimationCurve powerCurve;
    [SerializeField, MinMaxSlider(0, 3)] private Vector2 minMaxCharge;
    [SerializeField, MinMaxSlider(0, 100)] private Vector2 minMaxPower;
    private float CalculatePower(float timeCharged)
    {
        float t = powerCurve.Evaluate(NormalizeValue(timeCharged, minMaxCharge.x, minMaxCharge.y));
        float finalPower = Mathf.Lerp(minMaxPower.x, minMaxPower.y, t);
        Debug.Log("Power: " + finalPower);
        return finalPower;
    }

    private float NormalizeValue(float v, float min, float max)
    {
        return (v - min) / (max - min);
    }
    
}