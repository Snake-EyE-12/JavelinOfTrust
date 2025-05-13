using System;
using CharacterProcess.Gimmicks;
using UnityEngine;

[Serializable]
public class CharacterAttackingGimmick : ICharacterGimmick
{
    [SerializeField] public Javelin javelinPrefab;
    [SerializeField] public float slowdownMultiplier;
    /**/
    [NonSerialized] public float timeOfAttackStart;
    [NonSerialized] public bool attacking;
}

public class StartAttackListener : BaseCharacterProcessor
{
    public override void Operate(CharacterDataSettings data)
    {
        if (data.input.Input.attack.Down)
        {
            data.attack.timeOfAttackStart = Time.time;
            data.attack.attacking = true;
        }
    }
}
public class ApplyAimingSlowness : BaseCharacterProcessor
{
    public override void Operate(CharacterDataSettings data)
    {
        if (!data.attack.attacking) return;
        data.locomotion.Acceleration.Multiplicative.x *= data.attack.slowdownMultiplier;
    }
}

public class Attack : BaseCharacterProcessor
{
    public override void Operate(CharacterDataSettings data)
    {
        if (data.input.Input.attack.Up && data.attack.attacking)
        {
            Vector2 inputDirection = data.input.Input.direction;
            if (inputDirection == Vector2.zero) inputDirection = new Vector2(data.input.lastMovementDirection.x, 0);
            Javelin javelin = GameObject.Instantiate(data.attack.javelinPrefab, data.physics.RigidBody.transform.position, data.physics.RigidBody.transform.rotation, data.physics.RigidBody.transform);
            javelin.Throw(Time.time - data.attack.timeOfAttackStart, data.locomotion.Velocity.Value, data.physics.RigidBody.transform, inputDirection);
            data.attack.attacking = false;
        }
    }
}