using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Player : Character
{
    // Start is called before the first frame update
    public PlayerData Data;
    #region Variables
    public Rigidbody2D RB { get; private set; }
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsWallJumping { get; private set; }
    public bool IsSliding { get; private set; }

    //Timers (also all fields, could be private and a method returning a bool could be used)
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }

    //Jump
    private bool _isJumpCut;
    private bool _isJumpFalling;

    //Wall Jump
    private float _wallJumpStartTime;
    private int _lastWallJumpDir;

    private Vector2 _moveInput;
    public float LastPressedJumpTime { get; private set; }

    //Set all of these up in the inspector
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _waterLayer;
    public float knockbackForce;
    public float shootForce;
    public static Player Instance;
    public GameObject hitbox;
    public GameObject firesSpark;
    public GameObject Apple;
    public bool attack = false;
    public bool isMoing = false;
    public bool isParry = false;
    public bool KbFromR = false;
    public bool isHurt = false;
    public bool healing = false;

    public float healTimer;
    public float timeToHeal;

    public float KbTotalTime;
    public float KbCounter;
    
    public Transform StartPos;
    private float _fallSpeedYDampingChangeThrehold;
    private CinemachineImpulseSource _imPulse;
    [Space(5)]
   
    [Header("Mana Setting")]
    public float mana;
    [SerializeField] float manaDrainSpeed;
    public float manaGain;

    [SerializeField] public  Image manaStorage;


    #endregion



    public override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
          RB = GetComponent<Rigidbody2D>();
 
    }
    protected override void Start()
    {
        base.Start();
        SetGravityScale(Data.gravityScale);
        IsFacingRight = true;
        _fallSpeedYDampingChangeThrehold = CameraController.Instance._fallSpeedYDampingChangeThreshold;
        _imPulse = GetComponent<CinemachineImpulseSource>();
        
    }

    public override void OnInit()
    {
        base.OnInit();
        attack = false;
        isParry = false;
        hitbox.SetActive(false);
        Mana = mana;
        transform.position = StartPos.position;
        if(manaStorage != null)
        {
            manaStorage.fillAmount = Mana;
        }
        
    }

    public override void DesSpawn()
    {
        base.DesSpawn();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        ChangeAnim("die");
        
    }

    private void Update()
    {
        if (!IsDead)
        {
            if (isParry)
            {
                if (firesSpark != null)
                {
                    firesSpark.SetActive(true);
                    Invoke(nameof(unActive), 0.3f);

                }
            }
            else
            {
                firesSpark.SetActive(false);
            }
            ATK();
            Heal();
            #region TIMERS
            LastOnGroundTime -= Time.deltaTime;
            LastOnWallTime -= Time.deltaTime;
            LastOnWallRightTime -= Time.deltaTime;
            LastOnWallLeftTime -= Time.deltaTime;

            LastPressedJumpTime -= Time.deltaTime;
            #endregion

            #region INPUT HANDLER
            _moveInput.x = Input.GetAxisRaw("Horizontal");
            _moveInput.y = Input.GetAxisRaw("Vertical");

            if (_moveInput.x != 0)
                CheckDirectionToFace(_moveInput.x > 0);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.J))
            {
                OnJumpInput();
            }

            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.J))
            {
                OnJumpUpInput();
            }
            #endregion


            #region COLLISION CHECKS
            if (!IsJumping)
            {
                //Ground Check
                if (Physics2D.OverlapCircle(_groundCheckPoint.position, 0.2f, _groundLayer) && !IsJumping) //kiểm tra xem hộp thiết lập có chồng lên mặt đất không
                {
                    LastOnGroundTime = Data.coyoteTime; //nếu vậy đặt lastGrounded thành coyoteTime
                }
            }
            #endregion


            #region JUMP CHECKS
            if (IsJumping && RB.velocity.y < 0)
            {
                IsJumping = false;
                ChangeAnim("fall");
                if (!IsWallJumping)
                    _isJumpFalling = true;
            }

            if (IsWallJumping && Time.time - _wallJumpStartTime > Data.wallJumpTime)
            {
                IsWallJumping = false;
            }

            if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
            {
                _isJumpCut = false;

                if (!IsJumping)
                    _isJumpFalling = false;
            }

            //Jump
            if (CanJump() && LastPressedJumpTime > 0)
            {
                IsJumping = true;
                IsWallJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;
                Jump();
                ChangeAnim("jump");
            }
            /*  //WALL JUMP
              else if (CanWallJump() && LastPressedJumpTime > 0)
              {
                  IsWallJumping = true;
                  IsJumping = false;
                  _isJumpCut = false;
                  _isJumpFalling = false;
                  _wallJumpStartTime = Time.time;
                  _lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;
                  ChangeAnim("wallJump");
                  WallJump(_lastWallJumpDir);
                  Turn();

              }*/
            #endregion

            #region SLIDE CHECKS
            if (CanSlide() && ((LastOnWallLeftTime > 0 && _moveInput.x < 0) || (LastOnWallRightTime > 0 && _moveInput.x > 0)))
                IsSliding = true;
            else
                IsSliding = false;
            #endregion

            #region GRAVITY
            //Lực hấp dẫn cao hơn nếu chúng ta đã thả đầu vào nhảy hoặc đang rơi
            if (IsSliding)
            {
                SetGravityScale(0);
            }
            else if (RB.velocity.y < 0 && _moveInput.y < 0)
            {
                //Lực hấp dẫn cao hơn nhiều nếu giữ chặt
                SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
                //Giới hạn tốc độ rơi tối đa, vì vậy khi rơi từ khoảng cách lớn, chúng ta không tăng tốc đến tốc độ cao một cách điên cuồng
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
            }
            else if (_isJumpCut)
            {
                //Trọng lực cao hơn nếu thả nút nhảy
                SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
            {
                SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
            }
            else if (RB.velocity.y < 0)
            {
                //Trọng lực cao hơn nếu rơi
                SetGravityScale(Data.gravityScale * Data.fallGravityMult);
                // Giới hạn tốc độ rơi tối đa, vì vậy khi rơi từ khoảng cách lớn, chúng ta không tăng tốc đến tốc độ cao một cách điên cuồng
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else
            {
                //Lực hấp dẫn mặc định nếu đứng trên bệ hoặc di chuyển lên trên
                SetGravityScale(Data.gravityScale);
            }
            #endregion
           
        }
        else
        {
            OnDeath();
  
        }
  
   
    }
    void FixedUpdate()
    {
       
        if (!IsDead)
        {
            
            if (KbCounter <= 0)
            {
                //Handle Run
                if (IsWallJumping)
                    Run(Data.wallJumpRunLerp);
                else
                    Run(1);

                //Handle Slide
                if (IsSliding)
                    Slide();
                
            }
            else
            {
                ChangeAnim("hurt");
                if (KbFromR)
                {
                    RB.velocity = new Vector2(-knockbackForce, 0);

                }
                else 
                {
                    RB.velocity = new Vector2(knockbackForce, 0);
                }
                KbCounter -= Time.fixedDeltaTime;
            }
        }
        else
        {
            KbCounter = 0;
        }
        if (RB.velocity.y < _fallSpeedYDampingChangeThrehold && !CameraController.Instance.IsLerpingYDamping && !CameraController.Instance.LerpedFromPlayerFalling)
        {
            CameraController.Instance.LerpYDamping(true);
        
        }
        if (RB.velocity.y >= 0f && !CameraController.Instance.IsLerpingYDamping && CameraController.Instance.LerpedFromPlayerFalling)
        {
            CameraController.Instance.LerpedFromPlayerFalling = false;
          
            CameraController.Instance.LerpYDamping(false);
      
        }
       
    }
    private void ATK()
    {
       
        if (!isHurt && !attack &&!IsJumping)
        {
           
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ChangeAnim("atk");
                attack = true;
                hitbox.SetActive(true);
            }
            if(Input.GetKeyDown(KeyCode.E))
            {
                ChangeAnim("throw");
                attack = true;
            }
        }   
    }
    public void ShockWave()
    {
        
        Debug.Log("shockWave");
    }
    void Heal()
    {
       
        if(Input.GetKey(KeyCode.R) && hp< maxHp && Mana >0 &&!isHurt && !isMoing&& !attack&& !IsJumping) 
        {
      
            healTimer += Time.deltaTime;
            healing = true;
            ChangeAnim("healing");
            if (healTimer >= timeToHeal )
            {       
                hp +=1;
                healTimer = 0;
                onHealthChangeCallBack();
                VFXManager.Instance.CallShockWave();
                ScreenShakeManager.Instance.CameraShake(_imPulse);
            }
           
            // drain mana
            Mana -= Time.deltaTime*manaDrainSpeed;
        }
        else
        {
            healing =false;
            healTimer = 0;
            
          
        }
    }


    public void Throw()
    {
        if(Mana > 0)
        {
            VFXManager.Instance.CallShockWave();
            ScreenShakeManager.Instance.CameraShake(_imPulse);
            // Tạo ra mũi tên tại vị trí shootPoint
            GameObject apple = Instantiate(Apple, hitbox.transform.position, Quaternion.identity);
            Vector2 direction;
            // Lấy hướng từ vị trí bắn tới vị trí của người chơi
            if (IsFacingRight)
            {
                direction = Vector2.right;
            }
            else
            {
                direction = Vector2.left;
            }
            /*Mana -= 0.25f;*/
            // Gán lực cho mũi tên để bắn nó về phía người chơi
            Rigidbody2D rb = apple.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * shootForce, ForceMode2D.Impulse);
            Mana -= 0.1f;
        }
       
       
    }
    public void ResetState()
    {
        attack = false;
        isHurt  = false;
        hitbox.SetActive(false);
    }


    #region INPUT CALLBACKS
    //Methods which whandle input detected in Update()
    public void OnJumpInput()
    {
        LastPressedJumpTime = Data.jumpInputBufferTime;
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut() || CanWallJumpCut())
            _isJumpCut = true;
    }
    #endregion

    #region GENERAL METHODS
    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }
    #endregion

    //MOVEMENT METHODS
    #region RUN METHODS
    private void Run(float lerpAmount)
    {
        
        float targetSpeed = _moveInput.x * Data.runMaxSpeed;

        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate
        float accelRate;


        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
        #endregion

        #region Add Bonus Jump Apex Acceleration

        if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }
        #endregion

        #region Conserve Momentum

        if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {

            accelRate = 0;
        }
        #endregion
        if (Physics2D.OverlapCircle(_groundCheckPoint.position, 0.2f, _groundLayer) && !IsJumping && !attack)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            if (Mathf.Abs(targetSpeed) < 0.01f && Mathf.Abs(RB.velocity.x) < 0.01f )
            {
                // Nhân vật sẽ dừng lại.
                if (!healing)
                {
                    ChangeAnim("idle");
                }
                
            }
            else
            {
                ChangeAnim("run");
            }
        }
        if(Physics2D.OverlapCircle(_groundCheckPoint.position, 0.2f, _waterLayer) && !IsJumping && !attack && !Physics2D.OverlapCircle(_groundCheckPoint.position, 0.2f, _groundLayer))
        {
            
            if (Mathf.Abs(targetSpeed) < 0.01f && Mathf.Abs(RB.velocity.x) < 0.01f)
            {
                transform.rotation = Quaternion.Euler(0, 0,0);
            }
            else
            {
                ChangeAnim("run");
                transform.rotation = Quaternion.Euler(0, 0, -_moveInput.x * 30);
            }
        }



            float speedDif = targetSpeed - RB.velocity.x;


        float movement = speedDif * accelRate;


        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }

    private void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        IsFacingRight = !IsFacingRight;
    }
    #endregion

    #region JUMP METHODS
    private void Jump()
    {

        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        #region Perform Jump

        float force = Data.jumpForce;
        if (RB.velocity.y < 0)
            force -= RB.velocity.y;

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        ChangeAnim("jump");
        #endregion
    }

    private void WallJump(int dir)
    {
        //Đảm bảo chúng ta không thể gọi Wall Jump nhiều lần chỉ bằng một lần nhấn
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;
        
        #region Perform Wall Jump
        Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
        force.x *= dir; //áp dụng lực theo hướng ngược lại với tường

        if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
            force.x -= RB.velocity.x;

        if (RB.velocity.y < 0) //kiểm tra xem người chơi có đang rơi không, nếu có, chúng tôi trừ đi vận tốc.y(lực đối trọng).Điều này đảm bảo người chơi luôn đạt được lực nhảy mong muốn hoặc lớn hơn
            force.y -= RB.velocity.y;


        RB.AddForce(force, ForceMode2D.Impulse);
        #endregion
    }
    #endregion

    #region OTHER MOVEMENT METHODS
    private void Slide()
    {
        float speedDif = Data.slideSpeed - RB.velocity.y;
        float movement = speedDif * Data.slideAccel;
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        RB.AddForce(movement * Vector2.up);
    }
    #endregion
     private void onHurt(Vector2 attackDirection)
    {
        // Đẩy lùi player theo hướng ngược lại với đòn tấn công
        Vector2 knockbackDirection = new Vector2(-attackDirection.normalized.x, 0); // Giữ nguyên chiều ngang, không thay đổi chiều dọc
        RB.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        isHurt = true;
    }

    #region CHECK METHODS
    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }

    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }

    private bool CanWallJump()
    {
        return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
             (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
    }

    private bool CanJumpCut()
    {
        return IsJumping && RB.velocity.y > 0;
    }

    private bool CanWallJumpCut()
    {
        return IsWallJumping && RB.velocity.y > 0;
    }

    public bool CanSlide()
    {
        if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && LastOnGroundTime <= 0)
            return true;
        else
            return false;
    }
    #endregion


    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

       
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EHitBox") || collision.CompareTag("arrow"))
        {
            if (!isParry && !IsDead)
            {
                OnHit(1);
                if (collision.transform.position.x >= transform.position.x)
                {
                    KbFromR = true;
                
                }
                else
                {
                    KbFromR = false;
                   
                }

                KbCounter = KbTotalTime;
            }  
        }
    }

    private void unActive()
    {
        isParry= false;
        firesSpark.SetActive(false);
    }

    public float Mana
    {
        get { return mana; }
        set 
        { 
           if(mana != value)
           {
               mana = Mathf.Clamp(value,0,1);
                manaStorage.fillAmount = Mana;
            }
        
        }
    }

}
