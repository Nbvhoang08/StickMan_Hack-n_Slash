using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Enemies
{

    public GameObject arrowPrefab;   // Prefab của mũi tên
    public Transform shootPoint;     // Vị trí mà mũi tên sẽ được tạo ra      
    public float shootForce = 10f;
    public float shootCooldown = 1f; // Thời gian cooldown giữa các lần bắn
    private float lastShootTime = 0f; // Lưu trữ thời gian bắn gần nhất
    protected override void Start()
    {
        base.Start();
        changeState(new IdleState());

    }


    public override void OnInit()
    {
        base.OnInit();

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
    }

    public override void Cast()
    {
        base.Cast();
        StopMoving();
        ChangeAnim("bow");
        shoot();
    }



    //////
    void shoot()
    {
        if (Time.time - lastShootTime >= shootCooldown)
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
            rb.velocity = direction * shootForce;

            // Cập nhật thời gian bắn cuối cùng
            lastShootTime = Time.time;
        }
    }

    void Combo()
    {

    }



 }
