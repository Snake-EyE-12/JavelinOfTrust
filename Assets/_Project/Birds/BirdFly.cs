using System;
using UnityEngine;

public class BirdFly : MonoBehaviour
{
    public static Action OnBirdHit = delegate { };
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Transform boundsPos;
    [SerializeField] private Transform boundsStart;
    private void Update()
    {
        transform.position = transform.right * (speed * Time.deltaTime) + transform.position;

        //x
        if (transform.position.x < boundsPos.position.x) transform.position = new Vector3(boundsStart.position.x, transform.position.y, 0);
        
        //y
        if (transform.position.y > boundsPos.position.y) transform.position = new Vector3(transform.position.x, boundsStart.position.y, 0);
        
        
        //Move rotation in ping pong style
        transform.rotation *= Quaternion.Euler(0, 0, Mathf.PingPong(Time.time, rotationSpeed) - (rotationSpeed * 0.5f));
    }

    [SerializeField] private Rigidbody2D body;
    public void OnHit(Vector2 velocity)
    {
        body.linearVelocity = velocity;
        if (killed) return;
        OnBirdHit.Invoke();
        Die();
    }

    private bool killed;

    [SerializeField] private Sprite deadBird;
    private void Die()
    {
        body.bodyType = RigidbodyType2D.Dynamic;
        // stop animation
        GetComponent<Animator>().enabled = false;
        // set sprite to dead
        GetComponent<SpriteRenderer>().sprite = deadBird;
        // play sound
        // show particles
        // show hit
        // already hit
        killed = true;
        //rotation
        transform.rotation = Quaternion.Euler(0, 0, 0);

        this.enabled = false;
    }
}
