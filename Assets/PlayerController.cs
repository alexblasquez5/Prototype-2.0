using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 10;
    public float jumpForce = 50;
    public float dashForce = 20;
    public bool isGrounded;
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(x,y);

        Walk(direction);

        //Jump Button is Pressed
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        //Dash Button ('j') is pressed
        if (Input.GetKeyDown(KeyCode.J))
        {
            Dash(x, y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

    private void Walk(Vector2 direction)
    {
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void Dash(float x, float y)
    {
        //Resets the current velocity
        rb.velocity = Vector2.zero;

        // If no vertical input, only dash horizontal
        if (x != 0 && y == 0)
        {
            rb.velocity = new Vector2(x * dashForce, rb.velocity.y);
        }
        else{
            Vector2 direction = new Vector2(x, y).normalized;
            rb.velocity += direction * dashForce;
        }
    }
}
