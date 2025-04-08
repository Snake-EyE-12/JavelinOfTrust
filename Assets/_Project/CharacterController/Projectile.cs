using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private float timeOfThrow;
    private Vector2 throwVelocity;
    private Vector2 inheritedVelocity;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float inheritedVelocityPercent;
    [SerializeField] private float inheritedVelocityFallOffTime;


    public void Launch(Vector2 ownerVelocity, Vector2 direction, float power)
    {
        timeOfThrow = Time.time;
        throwVelocity = direction * power;
        inheritedVelocity = ownerVelocity;
    }

    private void Update()
    {
        float percent = Mathf.Clamp01(Time.time - timeOfThrow / inheritedVelocityFallOffTime);
        ApplyVelocity((1 - Mathf.Clamp01(percent)) * inheritedVelocityPercent);
    }

    private void ApplyVelocity(float percentInherited)
    {
        rigidBody.linearVelocity = inheritedVelocity * percentInherited + throwVelocity;
    }
}
