using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Đối tượng mà camera sẽ theo dõi
    public float smoothSpeed ;  // Tốc độ làm mượt chuyển động của camera
    public Vector3 offset;  // Khoảng cách giữa camera và target

    void FixedUpdate()
    {
        // Vị trí mong muốn của camera
        Vector3 desiredPosition = target.position + offset;

        // Dùng Lerp để làm mượt chuyển động
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Cập nhật vị trí của camera
        transform.position = smoothedPosition;
    }
}
