using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayBtn : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneName = "GamePlay";

    void Start()
    {
        // Tìm nút và gắn sự kiện OnClick
        Button playButton = GetComponent<Button>();
        playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    // Hàm sẽ được gọi khi nhấn nút Play
    void OnPlayButtonClicked()
    {
        // Chuyển đến cảnh khác (ở đây là sceneName)
        SceneManager.LoadScene(sceneName);
    }
}
