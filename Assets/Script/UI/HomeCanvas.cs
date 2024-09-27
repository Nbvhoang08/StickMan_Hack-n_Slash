using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeCanvas : UICanvas
{
    // Start is called before the first frame update
 
    SettingCanvas gameSettingCanvas;
    PlayCanvas gamePlayCanvas;
    DecorCanvas gameDecorCanvas;

    public void Awake()
    {
        gameSettingCanvas = FindObjectOfType<SettingCanvas>();
        if (gameSettingCanvas != null && gameSettingCanvas.gameObject.activeInHierarchy)
        {
            // Xử lý khi tìm thấy và đối tượng đang hoạt động
            gameSettingCanvas.gameObject.SetActive(false);
        }
       gamePlayCanvas = FindObjectOfType<PlayCanvas>();
        if(gamePlayCanvas != null && gamePlayCanvas.gameObject.activeInHierarchy)
        {
            gamePlayCanvas.gameObject.SetActive(false);
        }
        
        gameDecorCanvas = FindObjectOfType<DecorCanvas>();
        if(gameDecorCanvas != null && gameDecorCanvas.gameObject.activeInHierarchy)
        {
            gameDecorCanvas.gameObject.SetActive(true);
        }



    }
    public void PlayBtn()
    {
        UIManager.Instance.CloseAll();
        SenceController.Instance.ChangeScene("GamePlay");
        if (gameDecorCanvas)
        {
            gameDecorCanvas.gameObject.SetActive(false);
        }

        if (gamePlayCanvas)
        {
            gamePlayCanvas.gameObject.SetActive(true);
        }

    }

    private IEnumerator LoadGamePlayScene()
    {
        // Tải Scene bất đồng bộ
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GamePlay");

        // Chờ cho đến khi Scene tải xong
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Khi Scene đã tải xong, bật gamePlayCanvas
        if (gamePlayCanvas)
        {
            gamePlayCanvas.gameObject.SetActive(true);
        }
    }

    public void SettingBtn()
    {
        if (gameSettingCanvas)
        {
            gameSettingCanvas.gameObject.SetActive(true);
            UIManager.Instance.CloseUIDirectly<HomeCanvas>();
        }
        else
        {
            Debug.Log("null");
        } 
    }

    public void CreditBtn()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CreditCanvas>();
    }


}
