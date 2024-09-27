using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    // Start is called before the first frame update
    Dictionary<System.Type, UICanvas> canvasActives = new Dictionary<System.Type, UICanvas>();
    Dictionary<System.Type, UICanvas> canvasPrefabs = new Dictionary<System.Type, UICanvas>();
    [SerializeField] Transform parent;
    private bool isPaused = false;
    public Image manaStorge;

    protected override void Awake()
    {
        base.Awake();
        // Load tất cả các đối tượng UICanvas từ thư mục Resources/UI
        UICanvas[] prefabs = Resources.LoadAll<UICanvas>("UI");

        // Kiểm tra xem có tải được các đối tượng UI hay không
        if (prefabs.Length == 0)
        {
            return;
        }

        // Duyệt qua tất cả các đối tượng UICanvas vừa tải
        for (int i = 0; i < prefabs.Length; i++)
        {
            // Kiểm tra xem đối tượng có null hay không
            if (prefabs[i] == null)
            {
                continue;
            }

            // Thêm từng UICanvas vào dictionary canvasPrefabs
            // Sử dụng kiểu của UICanvas làm key và đối tượng UICanvas làm value
            if (!canvasPrefabs.ContainsKey(prefabs[i].GetType()))
            {
                canvasPrefabs.Add(prefabs[i].GetType(), prefabs[i]);
            }

        }
    }

    public T OpenUI<T>() where T : UICanvas
    {
        T canvas = GetUI<T>();
        canvas.Setup();
        canvas.Open();
        return canvas;
    }
    // mo UI co parent khac Canvas-Main
    public T OpenUI<T>(Transform customParent = null) where T : UICanvas
    {
        T canvas = GetUI<T>(customParent);
        canvas.Setup();
        canvas.Open();
        return canvas;
    }
    //dong canvas sau time
    public void CloseUI<T>(float time) where T : UICanvas
    {
        if (IsUILoaded<T>())
        {
            canvasActives[typeof(T)].Close(time);
        }
    }
    //dong canvas truc tiep
    public void CloseUIDirectly<T>() where T : UICanvas
    {
        if (OpenUI<T>())
        {
            canvasActives[typeof(T)].CloseDirectly();
        }
    }
    //kiem tra canvas da dc tao chua 
    public bool IsUILoaded<T>() where T : UICanvas
    {
        return canvasActives.ContainsKey(typeof(T)) && canvasActives[typeof(T)];
    }
    //kiem tra canvas da dc mo chua 
    public bool IsUIOpened<T>() where T : UICanvas
    {
        return IsUILoaded<T>() && canvasActives[typeof(T)].gameObject.activeSelf;
    }
    //lay canvas 
    public T GetUI<T>() where T : UICanvas
    {
        if (!IsUILoaded<T>())
        {
            T prefab = GetUIPrefab<T>();
            T canvas = Instantiate(prefab, parent);
            canvasActives[typeof(T)] = canvas;
        }
        return canvasActives[typeof(T)] as T;
    }
    public T GetUI<T>(Transform customParent = null) where T : UICanvas
    {
        if (!IsUILoaded<T>())
        {
            T prefab = GetUIPrefab<T>();
            T canvas = Instantiate(prefab, customParent);
            canvasActives[typeof(T)] = canvas;
        }
        return canvasActives[typeof(T)] as T;
    }

    public void ActiveUI<T>() where T : UICanvas
    {
        if (IsUIOpened<T>()) // Kiểm tra xem UI đã được mở hay chưa
        {
            canvasActives[typeof(T)].gameObject.SetActive(true); // Kích hoạt UI nếu đã được mở
        }
    }






    public T CreateNewUI<T>(Transform customParent = null) where T : UICanvas
    {
        T prefab = GetUIPrefab<T>();
        T canvas = Instantiate(prefab, customParent);
        return canvas;
    }
    private T GetUIPrefab<T>() where T : UICanvas
    {
        return canvasPrefabs[typeof(T)] as T;
    }
    public void CloseAll()
    {
        foreach (var canvas in canvasActives)
        {
            if (canvas.Value != null && canvas.Value.gameObject.activeSelf)
            {
                canvas.Value.Close(0);
            }
        }
    }


    public void PauseGame()
    {
        isPaused = !isPaused;

        // Thực hiện hành động tạm dừng game khi pause
        if (isPaused)
        {
            Time.timeScale = 0; // Dừng thời gian của gameplay
        }
        else
        {
            ResumeGame(); // Hàm để tiếp tục game khi nhấn nút "Resume" hoặc tương tự
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // Khôi phục thời gian bình thường


    }






    public void QuitGame()
    {
#if UNITY_EDITOR
        // Trong môi trường phát triển (Unity Editor)
        EditorApplication.isPlaying = false;
#else
        // Trong ứng dụng đã build
         Application.Quit();
#endif
    }
}


