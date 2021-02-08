using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public Rigidbody2D rb;

    public float jumpForce = 20f;
    public Transform feet;
    public LayerMask groundLayers;

    float startTime;
    float sign;
    float mx;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Horizontal"))
        {
            startTime = Time.time;
            sign = Input.GetAxisRaw("Horizontal");
        }

        if (Input.GetButtonUp("Horizontal") && isGrounded())
        {
            mx = sign * (Time.time - startTime);
            Jump();
        }

        
        /*
        if (Input.GetButtonDown("Jump") && isGrounded()){
            Jump();
            startTime = 0;
        }
        */
    }

    void FixedUpdate()
    {
        //Vector2 movement = new Vector2(mx * movementSpeed, rb.velocity.y);

        //rb.velocity = movement;
    }

    void Jump()
    {
        //Vector2 movement = new Vector2(rb.velocity.x, jumpForce);
        if(mx != 0){
            Vector2 movement = new Vector2(mx * movementSpeed, Mathf.Abs(mx * jumpForce));

            rb.velocity = movement;
        }
    }

    public bool isGrounded()
    {
        Collider2D groundCheck = Physics2D.OverlapCircle(feet.position, 0.5f, groundLayers);

        return groundCheck;
    }
}
