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
        return Mathf.Abs(a - b) < e;
    } 
    
    void OnCollisionEnter2D(Collision2D col)
    {
        //ContactPoint2D contact = col.GetContact(0);
        Vector2 point = col.GetContact(0).point;

        Collider2D playerCollider = col.otherCollider;
        char side = ' ';

        float xMinPoint = playerCollider.bounds.min.x;
        float xMaxPoint = playerCollider.bounds.max.x; 
        float yMinPoint = playerCollider.bounds.min.y; 
        float yMaxPoint = playerCollider.bounds.max.y; 

        if(Approximately(point.y, yMinPoint, 0.02f)){
            side = 'b';
        }
        else if(Approximately(point.y, yMaxPoint, 0.02f)){
            side = 't';
        }
        else if(Approximately(point.x, xMinPoint, 0.02f)){
            side = 'l';
        }
        else if(Approximately(point.x, xMaxPoint, 0.02f)){
            side = 'r';
        }
        
        Debug.Log(xMinPoint);
        Debug.Log(xMaxPoint);
        Debug.Log(yMinPoint);
        Debug.Log(yMaxPoint);
        Debug.Log(point);
        Debug.Log(side);
        
        if(side == 'l' || side == 'r'){
            Vector2 movement = new Vector2(col.relativeVelocity.x, rb.velocity.y);
            rb.velocity = movement;
        }
        else if(side == 't'){
            Vector2 movement = new Vector2(-1 * col.relativeVelocity.x, col.relativeVelocity.y);
            rb.velocity = movement;
        }
        else if (side == 'b'){
            Vector2 movement = new Vector2(0, 0);
            rb.velocity = movement;
            notJumping = true;
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
        return Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.02f, groundLayers) && notJumping && rb.velocity.y==0;
    }

    public bool HeadCollision()
    {
        return Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.up, 0.02f, groundLayers);
    }

    public bool IsStill()
    {
        return rb.velocity.y==0 && rb.velocity.x==0;
    }
}
