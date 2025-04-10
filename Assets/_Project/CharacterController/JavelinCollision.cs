using UnityEngine;

public class JavelinCollision : MonoBehaviour
{
    public Javelin javelin;
    public ContactFilter2D filter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!javelin.inAir) return;
        RaycastHit2D[] hits = new RaycastHit2D[1];
        Physics2D.CircleCast(javelin.transform.position, 0.5f, transform.position - javelin.transform.position, filter, hits);
        javelin.HitWall(hits[0].point);
    }
}