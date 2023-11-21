using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSpeed = 700f;
    [SerializeField] float jumpForce = 8f;

    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleMovementInput();
        CheckGrounded();
    }

    private void HandleMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(horizontal, 0f, vertical).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);
        }
        else
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f); // Reset velocity if there's no input
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
  