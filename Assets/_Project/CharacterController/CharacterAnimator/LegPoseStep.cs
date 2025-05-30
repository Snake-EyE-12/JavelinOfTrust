using UnityEngine;

public class LegPoseStep : CharacterLegState
{
    [SerializeField] private float stepSize;
    [SerializeField] private float duration;
    [SerializeField] private float height;
    [SerializeField] private LayerMask groundLayer;
    private Vector3 nextLeftPos;
    private Vector3 nextRightPos;
    private Vector2 previousLeftPos;
    private Vector2 previousRightPos;
    private float timeOfLeftStep;
    private float timeOfRightStep;
    [SerializeField] private float backstepOffset;
    [SerializeField] private float groundCheckDistance;
    public override void Exit()
    {
        
    }

    public override void Enter()
    {
        
    }

    private void StepLeftDirection()
    {
        // Step Left
        if (leftFoot.position.x > transform.position.x + backstepOffset)
        {
            previousLeftPos = nextLeftPos;
            timeOfLeftStep = Time.time;
            nextLeftPos = new Vector2(transform.position.x + leftPreset.x - stepSize, transform.position.y + leftPreset.y);
            var groundHit = Physics2D.Raycast(nextLeftPos, Vector2.down, groundCheckDistance, groundLayer);
            if (groundHit.collider != null)
            {
                nextLeftPos = groundHit.point;
            }
        }

        if (rightFoot.position.x > transform.position.x + backstepOffset)
        {
            previousRightPos = nextRightPos;
            timeOfRightStep = Time.time;
            nextRightPos = new Vector2(transform.position.x + rightPreset.x - stepSize, transform.position.y + rightPreset.y);
            var groundHit = Physics2D.Raycast(nextRightPos, Vector2.down, groundCheckDistance, groundLayer);
            if (groundHit.collider != null)
            {
                nextRightPos = groundHit.point;
            }
        }
    }

    private void StepRightDirection()
    {
        // Step Right
        if (leftFoot.position.x < transform.position.x - backstepOffset)
        {
            previousLeftPos = nextLeftPos;
            timeOfLeftStep = Time.time;
            nextLeftPos = new Vector2(transform.position.x + leftPreset.x + stepSize, transform.position.y + leftPreset.y);
            var groundHit = Physics2D.Raycast(nextLeftPos, Vector2.down, groundCheckDistance, groundLayer);
            if (groundHit.collider != null)
            {
                nextLeftPos = groundHit.point;
            }
        }

        if (rightFoot.position.x < transform.position.x - backstepOffset)
        {
            previousRightPos = nextRightPos;
            timeOfRightStep = Time.time;
            nextRightPos = new Vector2(transform.position.x + rightPreset.x + stepSize, transform.position.y + rightPreset.y);
            var groundHit = Physics2D.Raycast(nextRightPos, Vector2.down, groundCheckDistance, groundLayer);
            if (groundHit.collider != null)
            {
                nextRightPos = groundHit.point;
            }
        }
    }
    public override void Tick(CharacterDataSettings data)
    {
        if (data.locomotion.Velocity.Value.x < 0)
        {
            StepLeftDirection();
        }
        else
        {
            StepRightDirection();
        }

        UpdateActualPos();
    }

    [SerializeField] private AnimationCurve stepCurve;
    private void UpdateActualPos()
    {
        float lT = (Time.time - timeOfLeftStep) / duration;
        float rT = (Time.time - timeOfRightStep) / duration;

        float lX = Mathf.Lerp(previousLeftPos.x, nextLeftPos.x, lT);
        float lY = Mathf.Min(previousLeftPos.y, nextLeftPos.y) + (stepCurve.Evaluate(lT) * height);
        leftFoot.position = new Vector2(lX, lY);
        
        float rX = Mathf.Lerp(previousRightPos.x, nextRightPos.x, rT);
        float rY = Mathf.Min(previousRightPos.y, nextRightPos.y) + (stepCurve.Evaluate(rT) * height);
        rightFoot.position = new Vector2(rX, rY);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + leftPreset, transform.position + leftPreset + Vector3.right * stepSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + rightPreset, transform.position + rightPreset + Vector3.right * stepSize);
    }
}