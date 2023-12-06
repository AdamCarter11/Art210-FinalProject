using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSpeed = 700f;
    [SerializeField] float jumpForce = 8f;
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Animator playerAnimator;
    [SerializeField] GameObject boulder;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 spawnPoint;
    private bool lerpSpeed = false;
    private float startTime;
    GameObject spawnedBoulder;

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
        playerAnimator.SetFloat("Speed", Mathf.Abs(horizontal)); 
        if (horizontal > 0)
        {
            // flip facing right
            playerSprite.flipX = false;
        }
        else if(horizontal < 0)
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
            playerAnimator.SetTrigger("Jump");
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
        if (other.gameObject.CompareTag("JumpPad"))
        {
            other.gameObject.GetComponent<Animator>().SetTrigger("Spring");
            // Get the current velocity of the rigidbody
            Vector3 currentVelocity = rb.velocity;

            // Calculate the downward force based on the y velocity
            float downwardForce = -currentVelocity.y * 2f;

            // Add the downward force to the rigidbody
            rb.AddForce(new Vector3(0f, downwardForce, 0f), ForceMode.Impulse);
        }
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            other.gameObject.GetComponent<Animator>().SetTrigger("CheckPoint");
            spawnPoint = other.gameObject.transform.position;
            spawnedBoulder = Instantiate(boulder, new Vector3(other.transform.position.x, other.transform.position.y + 5f, other.transform.position.z), Quaternion.identity);
        }
        if (other.gameObject.CompareTag("Goal"))
        {
            // player wins
            print("Player wins!");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boulder"))
        {
            Destroy(spawnedBoulder);
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
  