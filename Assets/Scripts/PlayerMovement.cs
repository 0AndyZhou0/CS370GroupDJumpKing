using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider2d;
    public float MAX_SPEED = 1f;
    public Animator anim;

    public float jumpForce = 20f;
    public LayerMask groundLayers;

    float startTime;
    float direction;
    float chargeTime;

    bool readyToJump = false;
    bool chargingJump = false;
    bool notJumping = false;

    // Update is called once per frame
    void Update()
    {
        if(IsGrounded() && !chargingJump)
        {
            direction = Input.GetAxisRaw("Horizontal");
            /*
            if(direction == 0){
                Vector2 movement = new Vector2(0, rb.velocity.y);
                rb.velocity = movement;
            }
            */
        }
        
        if (IsStill() && IsGrounded() && Input.GetButtonDown("Jump") && !chargingJump)
        {
            chargingJump = true;
            startTime = Time.time;
        }

        if (IsGrounded() && Input.GetButtonUp("Jump") && chargingJump)
        {
            chargeTime = Time.time - startTime;
            //Debug.Log(chargeTime);
            if(chargeTime > MAX_SPEED){
                chargeTime = MAX_SPEED;
            }
            direction = Input.GetAxisRaw("Horizontal");
            chargingJump = false;
            readyToJump = true;
        }

        if (Mathf.Abs(direction) > 0.05f)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        if (direction > 0.05f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        anim.SetBool("isGrounded", IsGrounded());

        //Debug.Log(IsGrounded());
    }

    void FixedUpdate()
    {
        if(readyToJump && IsStill()){
            Jump();
        }


        if(IsGrounded() && !chargingJump){
            Vector2 movement = new Vector2(direction * movementSpeed, rb.velocity.y);
            rb.velocity = movement;
        }
    }

    bool Approximately(float a, float b, float e){
        //Debug.Log(Mathf.Abs(a - b));
        return Mathf.Abs(a - b) < e;
    } 
    
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(IsGrounded());
        if(!IsGrounded()){
            Vector2 normal = col.GetContact(0).normal;
            if(Approximately(normal.x, 0.0f, 0.01f) && Approximately(normal.y, 1.0f, 0.01f)){
                notJumping = true;
            }else{
                Vector2 velocity = col.relativeVelocity;
                //Debug.Log(normal);
                //Debug.Log(velocity);
                Vector2 newVelocity;
                float dotProduct = velocity.x * normal.x + velocity.y * normal.y;

                newVelocity.x = 2*(dotProduct)*normal.x - velocity.x;
                newVelocity.y = 2*(dotProduct)*normal.y - velocity.y;

                rb.velocity = newVelocity;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
    }

    void Jump()
    {
        //Vector2 movement = new Vector2(rb.velocity.x, jumpForce);
        if(chargeTime > 0){
            Vector2 movement = new Vector2(direction * movementSpeed, chargeTime * jumpForce);
            rb.velocity = movement;
        }
        notJumping = false;
        readyToJump = false;
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.02f, groundLayers) && notJumping;
    }

    public bool HeadCollision()
    {
        return Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.up, 0.02f, groundLayers);
    }

    public bool IsStill()
    {
        return Approximately(rb.velocity.x, 0.0f, 0.01f);
    }
}
