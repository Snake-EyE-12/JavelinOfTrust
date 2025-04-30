using UnityEngine;

namespace ProceduralCharacter
{
    public class StringDistanceLink : DistanceLink
    {
        protected override void Update()
        {
            if (Vector3.Distance(transform.position, anchor.position) > distance)
            {
                transform.position = anchor.position + (transform.position - anchor.position).normalized * distance;
            }
        }
    }
}