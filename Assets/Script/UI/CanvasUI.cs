using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasUI : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        UIManager.Instance.OpenUI<HomeCanvas>();
    }
    void OnEnable()
    {
        // Đăng ký sự kiện SceneManager.sceneLoaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Hủy đăng ký sự kiện SceneManager.sceneLoaded khi không cần thiết nữa
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Lấy đối tượng Camera của scene mới
        Camera newSceneCamera = Camera.main; // Hoặc bạn có thể lấy camera theo cách khác tùy theo thiết lập của bạn

        // Cập nhật lại eventCamera của canvas hoặc bất kỳ đối tượng nào cần truy cập camera
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.worldCamera = newSceneCamera;
        }
    }
}
