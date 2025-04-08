using UnityEngine;

public abstract class ItemDefinition : ScriptableObject
{
    public abstract void StartUse(ItemUseData data);

    public abstract void EndUse(ItemUseData data);
}


[CreateAssetMenu(menuName = "Inventory/Item", fileName = "Rock", order = 0)]
public abstract class RockItemDefinition : ItemDefinition
{
    public override void StartUse(ItemUseData data)
    {
        
    }

    public override void EndUse(ItemUseData data)
    {
        
    }
}


[CreateAssetMenu(menuName = "Inventory/Item", fileName = "Javelin", order = 0)]
public abstract class JavelinItemDefinition : ItemDefinition
{
    [SerializeField] private float timeBeforeUse;
    [SerializeField] private float halfPower;
    [SerializeField] private float fullPowerChargeTime;
    [SerializeField] private float fullPower;

    private float timeOfStartUse;

    public override void StartUse(ItemUseData data)
    {
        timeOfStartUse = Time.time;
    }

    public override void EndUse(ItemUseData data)
    {
        float heldDuration = Time.time - timeOfStartUse;

        if (heldDuration < timeBeforeUse)
        {
            return;
        }

        if (heldDuration < fullPowerChargeTime)
        {
            //Throw at low power
            //ThrowProjectile(CalculateVelocity(data.useDirection, halfPower, data.ownerVelocity), data.owner);
            return;
        }
        
        //ThrowProjectile(CalculateVelocity(data.useDirection, fullPower, data.ownerVelocity), data.owner);
        
        //Throw at full power

    }
    // private void ThrowProjectile(ProjectileVelocityData velocityData, Transform parent)
    // {
    //     //Instantiate(projectile, parent).Initialize(velocityData);
    // }
}

public class ItemUseData
{
    public Transform owner;
    public Vector2 useDirection;
    public Vector2 ownerVelocity;

    public ItemUseData(Transform _owner, Vector2 _useDirection, Vector2 _velocity)
    {
        owner = _owner;
        useDirection = _useDirection;
        ownerVelocity = _velocity;
        
    }
}