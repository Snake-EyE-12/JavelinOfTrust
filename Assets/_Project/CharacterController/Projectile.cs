using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private ProjectileData data;
    

    public void Initialize(ProjectileVelocityData initialVel)
    {
        //body.linearVelocity = initialVel;
    }
    // Starting Velocity
    // Additional Velocity due to Thrower
    // Fall off (Thrower Velocity)
    // Time of Fall Off
    // Maximum Speed
    // Maximum Distance
    // 
}

public abstract class ProjectileProcessor : BaseProcessor<ProjectileData>
{
    
}

public class InitialVelocitySetter : ProjectileProcessor
{
    public override void Process(ProjectileData data)
    {
        
        base.Process(data);
    }
}
public class LinearVelocityApplicator : ProjectileProcessor
{
    public override void Process(ProjectileData data)
    {
        data.rigidBody.linearVelocity = data.velocity;
        
        base.Process(data);
    }
}

[Serializable]
public class ProjectileData
{
    public Rigidbody2D rigidBody;
    [HideInInspector] public Vector2 velocity;
    [HideInInspector] public Vector2 throwVelocity;
    [HideInInspector] public Vector2 inheritedVelocity;
}