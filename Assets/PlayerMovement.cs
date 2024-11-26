using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private float maxJumpVelocity = 10f; // Maximum upward velocity
    private bool isFacingRight = true;
    private bool isJumping = false;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 25f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 0.4f;

    private bool isDamaged = false;

    private bool isWallSliding;
    private float wallSlidingSpeed = 1f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.2f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    private bool ignoreVelocityCap = false;

    public bool isOnPlatform;
    [HideInInspector]
    public Rigidbody2D platformRb;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    // Animator reference
    private Animator animator;

    private Vector2 previousPlatformVelocity;

    void Awake()
    {
        // Initialize Animator component
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        // Start jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            isJumping = true;
        }

        // Reduce jump height if the jump button is released early
        if (Input.GetKeyUp(KeyCode.Space) && isJumping)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); // Reduce upward velocity
            }
            isJumping = false;
        }

        if (!ignoreVelocityCap && rb.velocity.y > maxJumpVelocity)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxJumpVelocity);
        }

        if (Input.GetKeyDown(KeyCode.J) && canDash)
        {
            StartCoroutine(Dash());
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }

        // Update Animator parameters
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (isDashing || isWallJumping)
        {
            return;
        }

        if (isOnPlatform && platformRb != null)
        {
            // Add platform velocity if assigned
            rb.velocity = new Vector2(horizontal * speed + platformRb.velocity.x, rb.velocity.y);
        }

        else
        {
            // Fallback to normal behavior
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

   private void UpdateAnimator()
{
    animator.SetFloat("speed", Mathf.Abs(horizontal));
    animator.SetBool("isJumping", isJumping);
    animator.SetFloat("verticalSpeed", rb.velocity.y);
    animator.SetBool("isDashing", isDashing);
    animator.SetBool("isWallSliding", isWallSliding);
    animator.SetBool("isDamaged", isDamaged);
}

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void ActivateJumpPad(float upwardVelocity)
    {
        ignoreVelocityCap = true;
        rb.velocity = new Vector2(rb.velocity.x, upwardVelocity);
        StartCoroutine(ResetVelocityCap());
    }

    private IEnumerator ResetVelocityCap()
    {
        yield return new WaitForSeconds(0.1f);
        ignoreVelocityCap = false;
    }

    private IEnumerator Dash()
    {
    canDash = false;
    isDashing = true;
    animator.SetBool("isDashing", true); // Trigger Dash animation
    float originalGravity = rb.gravityScale;
    rb.gravityScale = 0f;
    rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
    tr.emitting = true;
    yield return new WaitForSeconds(dashingTime);
    tr.emitting = false;
    rb.gravityScale = originalGravity;
    isDashing = false;
    animator.SetBool("isDashing", false); // End Dash animation
    yield return new WaitForSeconds(dashingCooldown);
    canDash = true;
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
{
    if (IsWalled() && !IsGrounded() && rb.velocity.y < 0)
    {
        isWallSliding = true;
        animator.SetBool("isWallSliding", true); // Trigger Wall Slide animation

        // Flip the character to face away from the wall
        if (transform.position.x < wallCheck.position.x && isFacingRight)
        {
            Flip();
        }
        else if (transform.position.x > wallCheck.position.x && !isFacingRight)
        {
            Flip();
        }

        // Apply sliding physics
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
    }
    else
    {
        isWallSliding = false;
        animator.SetBool("isWallSliding", false); // End Wall Slide animation
    }
}


    private void WallJump()
{
    if (isWallSliding)
    {
        // Determine jump direction based on wall position
        wallJumpingDirection = transform.position.x > wallCheck.position.x ? 1 : -1; 
        wallJumpingCounter = wallJumpingTime;

        CancelInvoke(nameof(StopWallJumping));
    }
    else
    {
        wallJumpingCounter -= Time.deltaTime;
    }

    if (Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f)
    {
        isWallJumping = true;
        rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
        wallJumpingCounter = 0f;

        Invoke(nameof(StopWallJumping), wallJumpingDuration);
    }

    // Reset wall jumping if hitting a platform above
    if (IsTouchingCeiling())
    {
        StopWallJumping();
    }
}


    private bool IsTouchingCeiling()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.up;
        float distance = 0.5f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        return hit.collider != null;
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
        wallJumpingCounter = 0f;
    }


    public void TakeDamage()
{
    isDamaged = true;
    animator.SetBool("isDamaged", true); // Trigger Damage animation
    StartCoroutine(EndDamageState());
}

private IEnumerator EndDamageState()
{
    yield return new WaitForSeconds(0.5f); // Adjust time based on your damage animation length
    isDamaged = false;
    animator.SetBool("isDamaged", false); // End Damage animation
}
}
