using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    float movementSpeed = 2.0f;
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider2d;
    float MIN_SPEED = 0.4f;
    float MAX_SPEED = 1.2f;
    float GAME_SPEED = 1.7f;

    public Animator anim;

    //[SerializeField] private Transform groundCheckTransform = null;
    [SerializeField] private LayerMask groundLayerMask;

    public float jumpForce = 9f;
    public LayerMask groundLayers;

    float startTime;
    float direction;
    float chargeTime;

    bool readyToJump = false;
    bool chargingJump = false;
    bool notJumping = false;

    bool cheatsActive = false;
    public GameObject bar;
    public GameObject barBackground;
    float chargeLevel;

    bool paused;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    void Start()
    {
        Time.timeScale = GAME_SPEED;
        SkinSelect(PlayerPrefs.GetInt("skin"));

        if(PlayerPrefs.HasKey("x-position")){
            Vector2 prevPosition = new Vector2(PlayerPrefs.GetFloat("x-position"), PlayerPrefs.GetFloat("y-position"));
            transform.position = prevPosition;
            Vector2 prevVelocity = new Vector2(PlayerPrefs.GetFloat("x-velocity"), PlayerPrefs.GetFloat("y-velocity"));
            rb.velocity = prevVelocity;
        }

        if(PlayerPrefs.HasKey("cheats")){
            if(PlayerPrefs.GetInt("cheats") == 1){
                ToggleCheat();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsGrounded() && Input.GetKeyDown(KeyCode.Escape)){
            if(paused){
                Resume();
            }else{
                Pause();
            }
        }

        //Save
        PlayerPrefs.SetFloat("x-position", transform.position.x);
        PlayerPrefs.SetFloat("y-position", transform.position.y);
        PlayerPrefs.SetFloat("x-velocity", rb.velocity.x);
        PlayerPrefs.SetFloat("y-velocity", rb.velocity.y);

        if(Input.GetKeyDown(KeyCode.E))
        {
            SavePlayer();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            LoadPlayer();
        }

        if (!paused){

            //Movement

            if (chargingJump) {
                barBackground.transform.localScale = new Vector3(0.1f, 1.0f, 1.0f);
                chargeLevel = Time.time - startTime;
                if (chargeLevel > MAX_SPEED - MIN_SPEED) {
                    bar.transform.localScale = new Vector3(0.1f, 1.0f, 1.0f);
                    bar.transform.localPosition = new Vector3(-1.93f, -2.5f, 0.0f);
                }
                else if (chargeLevel > 0.0f) {
                    chargeLevel /= 0.8f;
                    bar.transform.localScale = new Vector3(0.1f, chargeLevel, 1.0f);
                    bar.transform.localPosition = new Vector3(-1.93f, -3.0f + (chargeLevel / 2), 0.0f);
                }
            } else {
                bar.transform.localScale = new Vector3(0.1f, 0.0f, 1.0f);
                barBackground.transform.localScale = new Vector3(0.1f, 0.0f, 1.0f);
            }

            if (IsGrounded() && !chargingJump)
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
                chargeTime += MIN_SPEED;
                if (chargeTime > MAX_SPEED) {
                    chargeTime = MAX_SPEED;
                }
                direction = Input.GetAxisRaw("Horizontal");
                chargingJump = false;
                readyToJump = true;
            }




            //Animation

            if (Mathf.Abs(direction) > 0.05f)
            {
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
            }
            if (direction > 0.0f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (direction < 0.0f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            anim.SetBool("isGrounded", IsGrounded());
            anim.SetBool("chargingJump", chargingJump);


            //Debug.Log(IsGrounded());
            //Debug.Log(notJumping);
        }
    }

    void FixedUpdate()
    {

        if (readyToJump && IsStill()) {
            Jump();
        }

        if (Approximately(rb.velocity.y, 0.0f, 0.01f)) {
            notJumping = true;
        } else {
            notJumping = false;
        }

        if (IsGrounded() && !chargingJump) {
            Vector2 movement = new Vector2(direction * movementSpeed, 0);
            rb.velocity = movement;
        }
    }






    //Math

    bool Approximately(float a, float b, float e) {
        //Debug.Log(Mathf.Abs(a - b));
        return Mathf.Abs(a - b) < e;
    }






    //Jump

    void Jump()
    {
        //Vector2 movement = new Vector2(rb.velocity.x, jumpForce);
        if (chargeTime > 0) {
            Vector2 movement = new Vector2(direction * 4.0f, chargeTime * jumpForce);
            rb.velocity = movement;
        }
        notJumping = false;
        readyToJump = false;
    }











    // Collision Detection

    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log(IsGrounded());
        if (!IsGrounded()) {
            Vector2 normal = col.GetContact(0).normal;
            //Debug.Log(normal);

            //flat ground
            if (Approximately(normal.x, 0.0f, 0.01f) && Approximately(normal.y, 1.0f, 0.01f)) {
                notJumping = true;
            }
            //walls
            else if((Approximately(normal.x, 1.0f, 0.01f) || Approximately(normal.x, -1.0f, 0.01f)) && Approximately(normal.y, 0.0f, 0.01f)){
                //Debug.Log("wall");
                Vector2 velocity = col.relativeVelocity;
                velocity.y *= -1;

                rb.velocity = velocity;
            }
            /*
            //flat roof
            else if (Approximately(normal.x, 0.0f, 0.01f) && Approximately(normal.y, -1.0f, 0.01f)) { }
            else {
                Vector2 velocity = col.relativeVelocity;
                //Debug.Log(velocity);
                Vector2 newVelocity;
                float dotProduct = velocity.x * normal.x + velocity.y * normal.y;

                newVelocity.x = 2 * (dotProduct) * normal.x - velocity.x;
                newVelocity.y = 2 * (dotProduct) * normal.y - velocity.y;

                rb.velocity = newVelocity;
            }
            */
        }
    }

    //void OnCollisionExit2D(Collision2D col){}


    public bool IsGrounded()
    {
        //Debug.Log(notJumping);
        return Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.02f, groundLayers) && notJumping;
        //return Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.01f, groundLayers);
    }

    /*
    public bool HeadCollision()
    {
        return Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.up, 0.02f, groundLayers);
    }
    */

    public bool IsStill()
    {
        return Approximately(rb.velocity.x, 0.0f, 0.01f);
    }










    //Pause Menu

    public void Resume(){
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        Time.timeScale = GAME_SPEED;
        paused = false;
    }

    public void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void Settings(){
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }

    public void Menu(){
        SceneManager.LoadScene("TitleScreenBasic");
    }

    //Settings
    public void ChangeSkin(){
        anim.Rebind();
        //anim.Update(0f);
        if(PlayerPrefs.GetInt("skin") == 1){
            PlayerPrefs.SetInt("skin", 0);
            SkinSelect(0);
        }else{
            PlayerPrefs.SetInt("skin", 1);
            SkinSelect(1);
        }
    }

    public void ToggleCheat(){
        if(cheatsActive){
            bar.SetActive(false);
            barBackground.SetActive(false);
            cheatsActive = false;
            PlayerPrefs.SetInt("cheats", 0);
        }else{
            bar.SetActive(true);
            barBackground.SetActive(true);
            cheatsActive = true;
            PlayerPrefs.SetInt("cheats", 1);
        }
    }






    //Skins
    void SkinSelect(int skinSelect){
        switch (skinSelect)
        {
            case 0:
                anim.SetBool("GreyGuySelect", true);
                anim.SetBool("GhostSelect", false);
                break;
            case 1:
                anim.SetBool("GreyGuySelect", false);
                anim.SetBool("GhostSelect", true);
                break;
        }
    }


    // Save and Load Functions

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;

    }

}



