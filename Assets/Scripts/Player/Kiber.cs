using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiber : MonoBehaviour
{
    //Public fields
    public ParticleSystem dust;
    public ParticleSystem splash;
    
    //Private fields
    Rigidbody2D rb;
    Animator anim;
    AudioSource audioSource;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform wallCheckCollider;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landSound;
    [SerializeField] AudioClip splashSound;
    [SerializeField] AudioClip outWater;

    const float groundCheckRadious = 0.02f;
    const float wallCheckRadius = 0.02f;
    [SerializeField] float speed;
    float startSpeed;
    [SerializeField] float jumpPower = 300;
    public float bounceForce;
    [SerializeField] float slideFactor = 0.4f;
    [SerializeField] int totalJumps;
    int availableJumps;
    float horizontalValue;
    float runSpeedModifier = 2f;

    [SerializeField] bool isInWater = false;
    [SerializeField] bool isGrounded;
    bool isRunning;
    bool facingRight = true;
    bool multipleJump;
    bool isSliding;
    bool coyoteJump;
    bool isDead = false;
    bool isBouncing = false;

    //public fields
    public float knockback;
    public float knockbackLength;
    public float knockbackCount;
    public bool knockFromRight;

    private void Awake()
    {
        availableJumps = totalJumps;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startSpeed = speed;
    }

    private void Update()
    {
        if(!CanMove())
            return;

        // Set yVelocity in the animator
        anim.SetFloat("yVelocity", rb.velocity.y);

        //Store horizontal value
        horizontalValue = Input.GetAxisRaw("Horizontal");

        //If running key is clicked then run forrest run!
        if (Input.GetButtonDown("Fire3"))
        {
            isRunning = true;
            CreateDust();
        }
        //If running key is released then stop running.
        if (Input.GetButtonUp("Fire3"))
        {
            isRunning = false;
            //CreateDust();
        }
            
        //If we press jump button enable jump
        if (Input.GetButtonDown("Jump"))
            Jump();
        
            
        //Check if we are on wall
        //WallCheck();
    }

    void Jump()
    {
        if (isGrounded)
        {
            multipleJump = true;
            availableJumps --;

            rb.velocity += Vector2.up * jumpPower;
            anim.SetBool("Jump", true);
            CreateDust();
            SoundManager.instance.PlaySound(jumpSound);
        }
        else
        {
            if (coyoteJump)
            {
                multipleJump = true;
                availableJumps--;

                rb.velocity += Vector2.up * jumpPower;
                anim.SetBool("Jump", true);
                CreateDust();
                SoundManager.instance.PlaySound(jumpSound);
            }
            if (multipleJump && availableJumps > 0)
            {
                availableJumps--;

                rb.velocity = Vector2.up * jumpPower;
                anim.SetBool("Jump", false);
                anim.SetBool("DoubleJump", true);
                SoundManager.instance.PlaySound(jumpSound);
            }
        }


    }
    
    public void Bounce()
    {
        rb.velocity += Vector2.up * bounceForce;
        anim.SetBool("Jump", true);
        CreateDust();
        SoundManager.instance.PlaySound(jumpSound);
        //bouncing = false;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue);

        
    }

    bool CanMove()
    {
        bool can = true;

        if (isDead) 
            can = false;

        return can;
    }

    void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        //Check if the GroundCheckObject is colliding with other 2D colliders that are in the ground layer.
        //If yes (isGrounded true) else (isGrounded false)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadious, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
            if (!wasGrounded)
            {
                availableJumps = totalJumps;
                multipleJump = false;
                anim.SetBool("DoubleJump", false);
                anim.SetBool("Jump", false);
                SoundManager.instance.PlaySound(landSound);
                CreateDust();
                //Debug.Log("LANDED!");
            }
        }
        else
        {
            if (wasGrounded)
                StartCoroutine(CoyoteJumpDelay());
        }


        //As long as we are grounded the "Jump" bool is in the animator is disabled
        anim.SetBool("Jump", !isGrounded);
    }

    IEnumerator CoyoteJumpDelay()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(0.2f);
        coyoteJump = false;
    }

    void WallCheck()
    {
        //If we are touching a wall
        //and we are moving towards the wall
        //and we are falling
        //and we are not grounded
        //slide on the wall
        if (Physics2D.OverlapCircle(wallCheckCollider.position, wallCheckRadius, wallLayer)
            && Mathf.Abs(horizontalValue) > 0
            && rb.velocity.y < 0
            && !isGrounded)
        {
            if (!isSliding)
            {
                availableJumps = totalJumps;
                multipleJump = false;
            }
            Vector2 v = rb.velocity;
            v.y = -slideFactor;
            rb.velocity = v;
            isSliding = true;
            anim.SetBool("Sliding", true);
            anim.SetBool("Jump", false);

            if (Input.GetButtonDown("Jump"))
            {
                availableJumps--;

                rb.velocity = Vector2.up * jumpPower;
                anim.SetBool("Jump", true);
                //anim.SetBool("Sliding", false);
                anim.SetBool("DoubleJump", false);
            }
        }
        else
        {
            anim.SetBool("Sliding", false);
            isSliding = false;
        }
    }

    private void Move(float dir)
    {
        //If the player is grounded and pressed "Jump button" then Jump
       

        #region Move & Run
        if (knockbackCount <= 0)
        {
            //Set value of x using dir and speed
            float xVal = dir * speed * 60 * Time.fixedDeltaTime;
            //If we are running multiply with the running modifier
            if (isRunning)
                xVal *= runSpeedModifier;
            //Create Vec2 for the velocity
            Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
            //Set the player's velocity
            rb.velocity = targetVelocity;

            //If looking right and clicking left (flip to the left)
            if (facingRight && dir < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                facingRight = false;
                CreateDust();

                if (isRunning == true)
                    CreateDust();
            }
            //If looking left and clicking right (flip to the right)
            else if (!facingRight && dir > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                facingRight = true;
                CreateDust();

                //if (isRunning == true)
                //CreateDust();
            }

            //Debug.Log(rb.velocity.x);
            // (0 for idle, 2 for walking, 4 for running)
            // Set the float xVelocity according to thex value of the RigidBody2D velocity.
            anim.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        }
        else
        {
            if (knockFromRight)
                rb.velocity = new Vector2(-knockback, knockback);
            if (!knockFromRight)
                rb.velocity = new Vector2(knockback, knockback);
            knockbackCount -= Time.deltaTime;
        }
        
        #endregion
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            rb.interpolation = RigidbodyInterpolation2D.None;
        }

        if (collision.gameObject.tag == "Water")
        {
            isInWater = true;
            

            if (isInWater)
            {
                SoundManager.instance.PlaySound(splashSound);
                rb.gravityScale = 1;
                jumpPower = 4.5f;
                speed = 0.5f;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        if (collision.gameObject.tag == "Water")
        {
            isInWater = false;
            

            if (!isInWater)
            {
                SoundManager.instance.PlaySound(outWater);
                rb.gravityScale = 3;
                jumpPower = 6.5f;
                speed = 0.7f;
            }
        }
    }

    public void Die()
    {
        isDead = true;
        speed = 0;
    }

    public void Respawn()
    {
        if (isDead)
        {
            isDead = false;
            speed = startSpeed;
        }
    }

    void CreateDust()
    {
        if (!isInWater)
            dust.Play();
        else
            return;
    }

    

    
}
