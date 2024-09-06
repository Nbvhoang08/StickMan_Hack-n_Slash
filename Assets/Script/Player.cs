using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour 
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    
    public bool moving;
    [SerializeField] float speed;
    [SerializeField] private float jumpForce;
    public Animator anim;
    private String currentAnimName;
    public Vector2 fallDirection = new Vector2(1, -1); // Hướng rơi chéo
    public float fallSpeed = 0.5f; // Tốc độ rơi chéo
    public Transform groundCheck;
    public LayerMask groundLayer;
    float hor;
    Vector2 dir;
    public bool isFacingRight = true;
    public bool actack = false;
    public static Player Instance;

    public float wallJumpForceX = 5f; // Lực theo phương ngang khi nhảy tường
    public float wallJumpForceY = 7f; // Lực theo phương dọc khi nhảy tường
    public Transform wallCheck; // Điểm để kiểm tra nếu chạm tường
    public float wallCheckRadius = 0.1f;
    public LayerMask wallLayer; // Layer của tường

    bool isWallGrabbing = false;
    float wallGrabDuration = 1f; // Thời gian bám trên tường
    float wallGrabTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpForce = 10;

    }
    private void Update()
    {
        if(wallGrabTimer >0)
            wallGrabTimer -= Time.deltaTime;

    }
    void FixedUpdate()
    {
        hor = Input.GetAxisRaw("Horizontal");
        ATK();
        Jump();
        Flip();
        Move();
        WallJump();
        
    }
    private void Move()
    {
        if (IsGrounded() && !actack)
        {
            rb.velocity = new Vector2(hor * speed, rb.velocity.y);

            if (hor == 0 )
            {
                ChangeAnim("idle");
            }
            else
            {
                ChangeAnim("run");

            }
        }
        else
        {
            if (hor != 0) // Nếu có di chuyển ngang khi đang rơi
            {
                rb.velocity = new Vector2(hor * speed, rb.velocity.y) + fallDirection * fallSpeed;
            }
            else // Nếu không di chuyển ngang, chỉ rơi tự do
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            }

            if (rb.velocity.y <= 0)
            {
                ChangeAnim("onGround");

            }


        }
    }

    void Jump()
    {

        if (Input.GetButton("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            // nhay khi dang dung tren mat dat    
            ChangeAnim("jump");
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); // giam  toc khi nhay len 

        }
    }
    void WallJump()
    {
        // Kiểm tra nếu đang chạm vào tường và không đứng trên đất
        if (IsTouchingWall() && !IsGrounded() && !isWallGrabbing)
        {
            isWallGrabbing = true;
            wallGrabTimer = wallGrabDuration;
            rb.velocity = new Vector2(0, 0); // Giữ nhân vật không rơi
            rb.gravityScale = 0;
        }

        // Khi người chơi nhấn nhảy khi đang bám vào tường
        if (isWallGrabbing && Input.GetButton("Jump"))
        {
            rb.gravityScale = 1;
            rb.velocity = new Vector2(wallJumpForceX * (isFacingRight ? 1 : -1), wallJumpForceY);
            Debug.Log("fly");
            isWallGrabbing = false; // Ngừng bám vào tường sau khi nhảy
            ChangeAnim("fly");
         
        }

        // Nếu không nhấn nhảy và hết thời gian bám trên tường
        if (isWallGrabbing)
        {
            
            ChangeAnim("wallSlide");
            if (wallGrabTimer <= 0)
            {
                ChangeAnim("fall");
                Debug.Log(wallGrabTimer);
                isWallGrabbing = false;
                rb.gravityScale = 1;
                rb.velocity = new Vector2(rb.velocity.x, -fallSpeed); // Rơi xuống
            }
        }
    }


    private void ATK()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeAnim("atk");
            actack = true;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            ChangeAnim("idle");
            actack = false;
        }

    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private bool IsTouchingWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
    }
    private void Flip()
    {
       
        if (isWallGrabbing && IsTouchingWall())
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
       
        }
        else
        {
            if (isFacingRight && hor < 0f || !isFacingRight && hor > 0f)
            {
                ChangeAnim("turnAround");
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
    }

    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(animName);
        }
    }




}
