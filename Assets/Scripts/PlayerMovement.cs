using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider2d;
    public float MAX_SPEED = 1f;

    public float jumpForce = 10f;
    public LayerMask groundLayers;

    float startTime;
    float direction;
    float moveTime;

    bool readyToJump = false;
    bool chargingJump = false;

    // Update is called once per frame
    void Update()
    {
        if(isGrounded() && !chargingJump)
        {
            direction = Input.GetAxisRaw("Horizontal");
        }

        if(isStill() && isGrounded())
        {
            if (Input.GetButtonDown("Jump") && !chargingJump)
            {
                chargingJump = true;
                startTime = Time.time;
            }

            if (Input.GetButtonUp("Jump") && chargingJump)
            {
                moveTime = Time.time - startTime;
                Debug.Log(moveTime);
                if(moveTime > MAX_SPEED){
                    moveTime = MAX_SPEED;
                }
                direction = Input.GetAxisRaw("Horizontal");
                chargingJump = false;
                readyToJump = true;
            }
        }
    }

    void FixedUpdate()
    {
        if(readyToJump && isStill()){
            Jump();
        }

        Vector2 movement = new Vector2(direction * movementSpeed, rb.velocity.y);

        rb.velocity = movement;
    }

    void Jump()
    {
        //Vector2 movement = new Vector2(rb.velocity.x, jumpForce);
        if(moveTime > 0){
            Vector2 movement = new Vector2(direction * moveTime * movementSpeed, moveTime * jumpForce);
            rb.velocity = movement;
        }
        readyToJump = false;
    }

    public bool isGrounded()
    {
        return Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.01f, groundLayers);
    }

    public bool isStill()
    {
        return rb.velocity.y==0 && rb.velocity.x==0;
    }
}
