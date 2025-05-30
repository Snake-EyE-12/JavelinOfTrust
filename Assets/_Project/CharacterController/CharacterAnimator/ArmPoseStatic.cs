using UnityEngine;

public class ArmPoseStatic : CharacterArmState
{
    [SerializeField, Range(0, 1)] private float lerpSpeed;
    public override void Enter()
    {
    }
    
    public override void Exit()
    {
    }

    public override void Tick(CharacterDataSettings data)
    {
        leftHand.position = Vector3.Lerp(leftHand.position, leftPreset + transform.position, lerpSpeed);
        rightHand.position = Vector3.Lerp(rightHand.position, rightPreset + transform.position, lerpSpeed);
    }
}