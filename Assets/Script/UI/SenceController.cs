using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SenceController : Singleton<SenceController>
{
    public void ChangeScene(string sceneName)
    {
        // Đăng ký sự kiện gọi lại khi cảnh mới đã được tải xong
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Tải cảnh mới
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        /*// Di chuyển người chơi đến vị trí mới
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = spawnPoint.transform.position;
        }
        // Hủy đăng ký sự kiện để tránh lỗi gọi lại không cần thiết
        SceneManager.sceneLoaded -= OnSceneLoaded;*/
    }
}
