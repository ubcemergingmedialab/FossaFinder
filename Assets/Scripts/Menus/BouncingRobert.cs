using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingRobert : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 lastVel;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(speed, speed / 2, 0), ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        lastVel = rb.velocity;
    }

    void OnCollisionEnter(Collision col)
    {
        ContactPoint cp = col.contacts[0];
        lastVel = Vector3.Reflect(lastVel, cp.normal);
        rb.velocity = lastVel;
        rb.velocity = rb.velocity.normalized * speed; // prevents object from losing speed
    }
}
