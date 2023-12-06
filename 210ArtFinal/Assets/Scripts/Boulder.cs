using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float accelerationRate = 0.1f;
    [SerializeField] float maxSpeed = 4f;
    [SerializeField] float followDistance = 2f;

    private Rigidbody rb;

    void Start()
    {
        if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        } 
        rb = GetComponent<Rigidbody>();
        //rb.freezeRotation = true; // Optionally freeze rotation to prevent unwanted tilting
    }

    void Update()
    {
        if (target != null)
        {
            // Calculate the direction to the target
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // Optional: Keep the movement only in the x-z plane

            // If the object is not within the follow distance, add force towards the target
            if (direction.magnitude > followDistance)
            {
                direction.Normalize();
                float speed = Mathf.Clamp(rb.velocity.magnitude, 0f, maxSpeed);
                float forceMagnitude = accelerationRate * speed;

                // Calculate the force to be applied
                Vector3 force = direction * forceMagnitude;

                // Add force to the Rigidbody for movement with collision detection 
                rb.AddForce(force, ForceMode.Acceleration);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }
}
