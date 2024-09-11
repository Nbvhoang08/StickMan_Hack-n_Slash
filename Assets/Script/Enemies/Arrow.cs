using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        SetUpRotation();
        Invoke(nameof(despawn),1f);
    }

    // Hàm điều chỉnh góc xoay của mũi tên theo hướng di chuyển
    public void SetUpRotation()
    {
        // Kiểm tra vận tốc hiện tại của mũi tên
        Vector2 velocity = rb.velocity;

        // Tính toán góc xoay dựa trên hướng di chuyển
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        // Gán góc xoay cho mũi tên
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void despawn()
    {
      
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hitbox"))
        { 
            despawn();
        }
    }

}
