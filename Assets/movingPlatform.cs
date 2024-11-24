using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    public Transform posA, posB;
    public float speed;
    private Vector3 targetPos;

    private Rigidbody2D rb;
    private Vector3 moveDirection;

    private PlayerMovement PlayerMovement;
    private Rigidbody2D playerRb;

    private Vector2 previousPlatformVelocity;

    private void Awake()
    {
        // Attempt to find and assign Player components
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement = player.GetComponent<PlayerMovement>();
            playerRb = player.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogError("Player not found. Ensure the player GameObject is tagged 'Player'.");
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D missing on the moving platform. Add a Rigidbody2D component.");
        }
    }

    private void Start()
    {
        targetPos = posB.position;
        DirectionCalculate();
        previousPlatformVelocity = Vector2.zero;
    }

    private void Update()
    {
        // Check for platform reaching the target position and switch direction
        if (Vector2.Distance(transform.position, posA.position) < 0.05f)
        {
            targetPos = posB.position;
            DirectionCalculate();
        }

        if (Vector2.Distance(transform.position, posB.position) < 0.05f)
        {
            targetPos = posA.position;
            DirectionCalculate();
        }
    }

    private void FixedUpdate()
    {
        // Move the platform
        rb.velocity = moveDirection * speed;

        // Update player's velocity if on the platform
        if (PlayerMovement != null && PlayerMovement.isOnPlatform)
        {
            Vector2 platformVelocity = rb.velocity;

            // Adjust player's velocity with the platform's velocity
            Vector2 velocityAdjustment = platformVelocity - previousPlatformVelocity;
            playerRb.velocity += velocityAdjustment;

            previousPlatformVelocity = platformVelocity;
        }
    }

    void DirectionCalculate()
    {
        moveDirection = (targetPos - transform.position).normalized;
    }

  private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player") && PlayerMovement != null)
    {
        Debug.Log("Assigning platformRb to PlayerMovement.");
        PlayerMovement.isOnPlatform = true;
        PlayerMovement.platformRb = rb; // Assign the platform's Rigidbody
        previousPlatformVelocity = rb.velocity;
    }
    else
    {
        Debug.LogWarning("OnTriggerEnter2D: Player or PlayerMovement is missing!");
    }
}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerMovement != null)
        {
            Debug.Log("Player exited platform trigger.");
            PlayerMovement.isOnPlatform = false;
            PlayerMovement.platformRb = null; // Clear platform reference
            previousPlatformVelocity = Vector2.zero; // Reset platform velocity
        }
    }
}
 