using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] float swingSpeed = 180.0f; // Degrees per second
    [SerializeField] float swingRange = 45.0f; // Degrees

    private float currentAngle = 0.0f;
    private bool isMovingForward = true;

    void Update()
    {
        // Rotate the axe
        float deltaAngle = swingSpeed * Time.deltaTime * (isMovingForward ? 1.0f : -1.0f);
        currentAngle += deltaAngle;
        transform.rotation = Quaternion.Euler(currentAngle, 0.0f, 0.0f);

        // Check if the axe reached its swing limit
        if (Mathf.Abs(currentAngle) >= swingRange)
        {
            isMovingForward = !isMovingForward;
        }
    }
}
  