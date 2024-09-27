using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Gravity")]
    [HideInInspector] public float gravityStrength; //Lực hướng xuống (trọng lực) cần thiết để đạt được độ cao nhảy và thời gian đạt đến đỉnh của cú nhảy mong muốn.
    [HideInInspector] public float gravityScale; //Sức mạnh của trọng lực của người chơi dưới dạng một bội số của trọng lực (đặt trong ProjectSettings/Physics2D). 
                                                 //Cũng là giá trị mà rigidbody2D.gravityScale của người chơi được đặt.
    [Space(5)]
    public float fallGravityMult; //Hệ số nhân vào gravityScale của người chơi khi rơi.
    public float maxFallSpeed; //Tốc độ rơi tối đa (tốc độ cực đại) của người chơi khi rơi.
    [Space(5)]
    public float fastFallGravityMult; //Hệ số lớn hơn cho gravityScale của người chơi khi họ rơi và nhấn phím xuống.
                                      //Thường thấy trong các game như Celeste, cho phép người chơi rơi nhanh hơn nếu muốn.
    public float maxFastFallSpeed; //Tốc độ rơi tối đa (tốc độ cực đại) của người chơi khi thực hiện rơi nhanh hơn.

    [Space(20)]

    [Header("Run")]
    public float runMaxSpeed; //Tốc độ tối đa mà chúng ta muốn người chơi đạt được.
    public float runAcceleration; //Tốc độ mà người chơi tăng tốc đến tốc độ tối đa, có thể được đặt thành runMaxSpeed để tăng tốc tức thì, hoặc 0 để không có tăng tốc.
    [HideInInspector] public float runAccelAmount; //Lực thực tế (nhân với sự chênh lệch tốc độ) được áp dụng lên người chơi.
    public float runDecceleration; //Tốc độ mà người chơi giảm tốc từ tốc độ hiện tại, có thể được đặt thành runMaxSpeed để giảm tốc tức thì, hoặc 0 để không có giảm tốc.
    [HideInInspector] public float runDeccelAmount; //Lực thực tế (nhân với sự chênh lệch tốc độ) được áp dụng lên người chơi.
    [Space(5)]
    [Range(0f, 1)] public float accelInAir; //Hệ số nhân được áp dụng cho tốc độ tăng tốc khi người chơi ở trên không.
    [Range(0f, 1)] public float deccelInAir;
    [Space(5)]
    public bool doConserveMomentum = true;

    [Space(20)]

    [Header("Jump")]
    public float jumpHeight; //Độ cao của cú nhảy của người chơi.
    public float jumpTimeToApex; //Thời gian từ khi áp dụng lực nhảy đến khi đạt được độ cao nhảy mong muốn. Những giá trị này cũng kiểm soát trọng lực và lực nhảy của người chơi.
    public float jumpForce; //Lực thực tế (hướng lên) được áp dụng cho người chơi khi họ nhảy.

    [Header("Both Jumps")]
    public float jumpCutGravityMult; //Hệ số nhân để tăng trọng lực nếu người chơi thả nút nhảy khi vẫn đang nhảy.
    [Range(0f, 1)] public float jumpHangGravityMult; //Giảm trọng lực khi gần đạt đến đỉnh nhảy (chiều cao mong muốn).
    public float jumpHangTimeThreshold; //Tốc độ (gần bằng 0) mà người chơi sẽ trải nghiệm sự "treo" khi nhảy. Tốc độ y của người chơi gần bằng 0 ở đỉnh của cú nhảy (nghĩ về đồ thị parabol hoặc hàm bậc hai).
    [Space(0.5f)]
    public float jumpHangAccelerationMult;
    public float jumpHangMaxSpeedMult;

    [Header("Wall Jump")]
    public Vector2 wallJumpForce; //Lực thực tế (được chúng ta đặt) được áp dụng cho người chơi khi nhảy tường.
    [Space(5)]
    [Range(0f, 1f)] public float wallJumpRunLerp; //Giảm ảnh hưởng của việc di chuyển khi người chơi nhảy tường.
    [Range(0f, 1.5f)] public float wallJumpTime; //Thời gian sau khi nhảy tường mà tốc độ di chuyển của người chơi bị giảm.
    public bool doTurnOnWallJump; //Người chơi sẽ xoay mặt hướng về hướng nhảy tường.

    [Space(20)]

    [Header("Slide")]
    public float slideSpeed;
    public float slideAccel;

    [Header("Assists")]
    [Range(0.01f, 0.5f)] public float coyoteTime; //Thời gian gia hạn sau khi rơi khỏi tường mà người chơi vẫn có thể nhảy.
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime; //Thời gian gia hạn sau khi nhấn nút nhảy mà nhảy sẽ tự động được thực hiện khi yêu cầu (ví dụ: đang đứng trên mặt đất) được đáp ứng.

    
    // Unity Callback, được gọi khi inspector cập nhật
    private void OnValidate()
    {
        //Tính toán lực trọng lực sử dụng công thức (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Tính toán tỉ lệ trọng lực của rigidbody (trọng lực tương đối với giá trị trọng lực của Unity, xem project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;

        //Tính toán lực tăng tốc & giảm tốc khi chạy sử dụng công thức: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        //Tính toán lực nhảy sử dụng công thức (initialJumpVelocity = gravity * timeToJumpApex)
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

        #region  Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
        #endregion
    }

}
