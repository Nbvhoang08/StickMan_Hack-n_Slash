using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Soldier : Enemies
{

    public GameObject arrowPrefab;   // Prefab của mũi tên
    public Transform shootPoint;     // Vị trí mà mũi tên sẽ được tạo ra      
    public float shootForce = 2f;
    public float shootCooldown = 0.5f; // Thời gian cooldown giữa các lần bắn
    public bool isAttacking;
    public bool isShooting;


    
    protected override void Start()
    {
        base.Start();
        changeState(new PatrolState());

    }


    public override void OnInit()
    {
        base.OnInit();
        isAttacking = false;
        isShooting = false;
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
    public override void Atk()
    {
        base.Atk();
        if (!isAttacking)
        {
            StartCoroutine(Combo());
        }
    }

    public override void Cast()
    {
        base.Cast();
      
        isShooting = true;
        StopMoving();
        ChangeAnim("bow");
        shoot();
    }
    public override void StopMoving()
    {
        base.StopMoving();
        if (!isShooting)
        {
            ChangeAnim("idle");
        }

    }
    public override void Moving()
    {
        base.Moving();
        ChangeAnim("run");
    }



    //////
    void shoot()
    {
        if (isShooting)
        {
            // Tạo ra mũi tên tại vị trí shootPoint
            GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);

            // Lấy hướng từ vị trí bắn tới vị trí của người chơi
            Vector2 direction = (Target.transform.position - shootPoint.position).normalized;
            // xoay mui ten theo huong target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
          
            // Gán lực cho mũi tên để bắn nó về phía người chơi
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            rb.AddForce(direction*shootForce,ForceMode2D.Impulse);
        }
        
        
    }

  

    IEnumerator Combo()
    {
        isAttacking = true;

        // Đòn tấn công đầu tiên
        DashTowardsPlayer(Target.transform, 2f);
        ChangeAnim("atk");
        yield return new WaitForSeconds(0.5f);

        // Đòn tấn công thứ hai
        DashTowardsPlayer(Target.transform, 4f); ////
        ChangeAnim("atk1");
        yield return new WaitForSeconds(1f);

        isAttacking = false; // Reset trạng thái tấn công
        /*ChangeAnim("idle");*/

    }

    void DashTowardsPlayer(Transform playerTransform, float dashDistance)
    {
        // Tính hướng từ enemy tới player
        ChangedDirection(Target.transform.position.x > transform.position.x);
        Vector3 dashDirection = (playerTransform.position - transform.position).normalized;

        // Di chuyển enemy về phía player một khoảng dashDistance
        transform.position += dashDirection * dashDistance;
    }



}
