using UnityEngine;

public class JavelinCollision : MonoBehaviour
{
    public Javelin javelin;
    public ContactFilter2D filter;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Decay decay;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!javelin.inAir) return;
        RaycastHit2D[] hits = new RaycastHit2D[1];
        Physics2D.CircleCast(javelin.transform.position, 0.5f, transform.position - javelin.transform.position, filter, hits);
        if (other.gameObject.CompareTag("Bird"))
        {
            Vector2 storedVelocity = body.linearVelocity;
            if(other.TryGetComponent(out BirdFly bird))
            {
                bird.OnHit(storedVelocity);
            }
            javelin.HitMovingObject(other.gameObject.transform, hits[0].point);
        }
        else if (other.TryGetComponent(out Target target))
        {
            //If Moving in same direction as target (dot product of velocity and transform.right of target)
            if (Vector2.Dot(body.linearVelocity, target.transform.right) > 0)
            {
                javelin.HitWall(hits[0].point);
                target.OnHit(hits[0].point, javelin.GetPowerThrownPercent());
                RemoveDecay();
            }
            
        }
        javelin.HitWall(hits[0].point);
    }
    private void RemoveDecay() => decay.enabled = false;
}