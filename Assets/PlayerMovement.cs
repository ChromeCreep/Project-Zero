using System;

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //Variables important to the player moving.
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;
    
    //Variables important to the player jumping.
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private BoxCollider2D groundedChecker;
    bool isGrounded = true;

    //Variables to allow jump buffering.
    [Header("Jump Buffer Settings")]
    [SerializeField] private float jumpBufferTimer;
    private float originalJumpBufferTimer;
    bool jumpBuffered = false;
    bool hasJumped = false;
    [Header("Fast Fall Properties")]
    bool fastFallActive = false;
    [SerializeField] private float fastFallSpeed;
    private float originalGravityScale;
    [SerializeField] private float yVelocity;
    [Header("Coyote Time Settings")]
    [SerializeField] private float coyoteTimer;
    private float originalCoyoteTimer;
    bool coyoteTime = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        originalJumpBufferTimer = jumpBufferTimer;
        originalCoyoteTimer = coyoteTimer;
        originalGravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        //Basic Movement Logic
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
        
        rb.linearVelocity = new Vector2(horizontalAxis * moveSpeed, rb.linearVelocity.y);
        //Jump Logic
        CheckIfJumpIsBuffered();
        CheckIfCoyoteTime();
        
        yVelocity = rb.linearVelocity.y;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //If we press space, we jump or buffer our jump.
            jumpBuffered = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isGrounded == false)
            {
                //If we release space when not grounded, we fall back to the ground faster.
                fastFallActive = true;
            }
        }

        if ((coyoteTime && jumpBuffered) || (isGrounded && jumpBuffered))
        {
            //This contains all of our jump logic.
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
            jumpBufferTimer = 0;
            coyoteTimer = 0;
            isGrounded = false;
            hasJumped = true;
        }

        if (hasJumped && isGrounded)
        {
            hasJumped = false;
        }
        
        FastFall();
    }

    void FastFall()
    {
        if (fastFallActive && isGrounded == false)
        {
            {
                rb.gravityScale = fastFallSpeed;
            }
        }
    }

    void CheckIfJumpIsBuffered()
    {
        if (jumpBuffered)
        {
            jumpBufferTimer -= Time.deltaTime;
            if (jumpBufferTimer <= 0)
            {
                jumpBuffered = false;
            }
        }
        else if (jumpBuffered == false)
        {
            jumpBufferTimer = originalJumpBufferTimer;
        }
    }
    
    void CheckIfCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimer = originalCoyoteTimer;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (coyoteTimer > 0)
        {
            coyoteTime = true;
        }
        else
        {
            coyoteTime = false;
        }
    }

    [SerializeField] private ParticleSystem landingEffect;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
            landingEffect.Play();
            rb.gravityScale = originalGravityScale;
            fastFallActive = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
