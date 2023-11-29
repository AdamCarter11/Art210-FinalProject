using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSpeed = 700f;
    [SerializeField] float jumpForce = 8f;
    [SerializeField] SpriteRenderer playerSprite;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 spawnPoint;
    private bool lerpSpeed = false;
    private float startTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnPoint = transform.position;
    }

    private void Update()
    {
        if(!lerpSpeed)
            HandleMovementInput();
        CheckGrounded();

        if (lerpSpeed)
        {
            LerpSpeed();
        }
    }

    private void HandleMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if(horizontal > 0)
        {
            // flip facing right
            playerSprite.flipX = false;
        }
        else
        {
            // flip facing left
            playerSprite.flipX = true;
        }
        //float vertical = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(horizontal, 0f, 0f).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hammer"))
        {
            StartCoroutine(HandleCollision());
        }
    } 

    IEnumerator HandleCollision()
    {
        startTime = Time.deltaTime;
        lerpSpeed = true;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(.4f);
        lerpSpeed = false;
        // Teleport the player back to spawn
        transform.position = spawnPoint;
        // Reset time scale to normal
        Time.timeScale = 1f;
        //rb.useGravity = true;
    }
    void LerpSpeed()
    {
        float slowdownDuration = .7f;
        //rb.useGravity = false;
        //rb.velocity = Vector3.zero;
        float targetSlow = .5f;
        // Slow down time
        float t = (Time.time - startTime) / slowdownDuration;
        Time.timeScale = Mathf.Lerp(1, targetSlow, t);
    }
} 
  