using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public Rigidbody2D rb;
    public float MAX_SPEED = 0.5f;

    public float jumpForce = 20f;
    public Transform feet;
    public LayerMask groundLayers;

    float startTime;
    float sign;
    float moveTime;

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
            moveTime = Time.time - startTime;
            if(moveTime > MAX_SPEED){
                moveTime = MAX_SPEED;
            }
            moveTime *= sign;
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
        //Vector2 movement = new Vector2(moveTime * movementSpeed, rb.velocity.y);

        //rb.velocity = movement;
    }

    void Jump()
    {
        //Vector2 movement = new Vector2(rb.velocity.x, jumpForce);
        if(moveTime != 0){
            Vector2 movement = new Vector2(moveTime * movementSpeed, Mathf.Abs(moveTime * jumpForce));

            rb.velocity = movement;
        }
    }

    public bool isGrounded()
    {
        Collider2D groundCheck = Physics2D.OverlapCircle(feet.position, 0.5f, groundLayers);

        return groundCheck;
    }
}
