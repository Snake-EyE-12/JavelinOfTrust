public class LegPoseStatic : CharacterLegState
{
    public override void Enter()
    {
    }
    
    public override void Exit()
    {
    }

    public override void Tick(CharacterDataSettings data)
    {
        leftFoot.position = leftPreset + transform.position;
        rightFoot.position = rightPreset + transform.position;
    }
}