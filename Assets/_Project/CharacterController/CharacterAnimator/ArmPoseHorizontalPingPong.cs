using UnityEngine;

public class ArmPoseHorizontalPingPong : CharacterArmState
{
    [SerializeField, Range(0, 1)] private float lerpSpeed;
    [SerializeField] private float span;
    [SerializeField] private float pingPongSpeed;
    public override void Enter()
    {
    }
    
    public override void Exit()
    {
    }

    public override void Tick(CharacterDataSettings data)
    {
        leftHand.position = Vector3.Lerp(leftHand.position + Vector3.right * ((Mathf.PingPong(Time.time * pingPongSpeed, span)) - (span * 0.5f)), leftHandDestination, lerpSpeed);
    }
}