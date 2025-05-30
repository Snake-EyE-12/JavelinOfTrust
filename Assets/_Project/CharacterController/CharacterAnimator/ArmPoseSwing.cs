using UnityEngine;

public class ArmPoseSwing : CharacterArmState
{
    [SerializeField] private float leftSwingLength;
    [SerializeField] private float rightSwingLength;
    
    public override void Exit()
    {
        
    }

    public override void Enter()
    {
        
    }

    public override void Tick(CharacterDataSettings data)
    {
        if (data.locomotion.Velocity.Value.x < 0)
        {
            // Moving Left
            if (leftHand.position.x > transform.position.x) leftHand.position = new Vector2(transform.position.x + leftPreset.x - leftSwingLength, transform.position.y + leftPreset.y);
            if (rightHand.position.x > transform.position.x) rightHand.position = new Vector2(transform.position.x + rightPreset.x - rightSwingLength, transform.position.y + rightPreset.y);
            
        }
        else
        {
            // Moving Right
            if (leftHand.position.x < transform.position.x) leftHand.position = new Vector2(transform.position.x + leftPreset.x + leftSwingLength, transform.position.y + leftPreset.y);
            if (rightHand.position.x < transform.position.x) rightHand.position = new Vector2(transform.position.x + rightPreset.x + rightSwingLength, transform.position.y + rightPreset.y);
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + leftPreset, transform.position + leftPreset + Vector3.right * leftSwingLength);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + rightPreset, transform.position + rightPreset + Vector3.right * rightSwingLength);
    }
}