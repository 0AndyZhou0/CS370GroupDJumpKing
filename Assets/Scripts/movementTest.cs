using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementTest : MonoBehaviour
{
    public float movementSpeed;
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider2d;
    public float MAX_SPEED = 1f;

    public float jumpForce = 10f;
    public LayerMask groundLayerMask;

    float startTime;
    float moveTime;
    float direction;

    bool readyToJump = false;
    bool ableToWalk = true;

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");

        if(rb.velocity.y==0 && rb.velocity.x==0){
            if (Input.GetButtonDown("Jump"))
            {
                ableToWalk = false;
                startTime = Time.time;
            }

            if (Input.GetButtonUp("Jump") && isGrounded())
            {
                moveTime = Time.time - startTime;
                if(moveTime > MAX_SPEED){
                    moveTime = MAX_SPEED;
                }
                direction = Input.GetAxisRaw("Horizontal");
                readyToJump = true;
            }
        }
    }

    void FixedUpdate()
    {
        if(readyToJump){
            Jump();
            readyToJump = false;
            ableToWalk = true;
        }

        //Move Left and Right
        if(isGrounded() && ableToWalk){
            Vector2 movement = new Vector2(3 * direction * movementSpeed, rb.velocity.y);
            rb.velocity = movement;
        }
    }

    void Jump()
    {
        //Vector2 movement = new Vector2(rb.velocity.x, jumpForce);
        if(moveTime > 0){
            Vector2 movement = new Vector2(moveTime * direction, moveTime * jumpForce);
            rb.velocity = movement;
        }
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.01f);
    }
}
