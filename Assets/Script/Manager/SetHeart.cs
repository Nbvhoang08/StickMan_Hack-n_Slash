using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SetHeart : MonoBehaviour
{
    // Start is called before the first frame update
    Player player;
    private GameObject[] _heartUI;
    private Image[] _heart_F;
    public Transform heartsParent;
    public GameObject heartPrefab;


    IEnumerator WaitForPlayer()
    {
        while (Player.Instance == null)
        {
            yield return null; // Chờ 1 frame để đợi Player.Instance được khởi tạo
        }

        SetupUI();
    }

    void OnEnable()
    {
        StartCoroutine(WaitForPlayer());
    }

    void SetupUI()
    {
        player = Player.Instance;
        if (player)
        {
            _heartUI = new GameObject[Player.Instance.hp];
            _heart_F = new Image[Player.Instance.hp];
            Player.Instance.onHealthChangeCallBack += UpdateHUD;
            SpawnHeart();
            UpdateHUD();
        }
        else
        {
            Debug.Log("null");
        }
    }
    private void Awake()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
     

    }
    void setHeartUI()
    {
       
        for(int i = 0; i < _heartUI.Length; i++)
        {
            if (i < Player.Instance.hp)
            {
                _heartUI[i].SetActive(true);
            }
            
        }
    }
    void setHeartFill()
    {
       
        for (int i = 0; i < _heart_F.Length; i++)
        {
     
            if (i < Player.Instance.hp)
            {
                _heart_F[i].fillAmount = 1;
           
            }
            else
            {
                _heart_F[i].fillAmount = 0;
           
            }
        }
    }
    void SpawnHeart()
    {
        
        for (int i = 0; i < Player.Instance.hp; i++)
        {
           
            GameObject temp = Instantiate(heartPrefab);
            temp.transform.SetParent(heartsParent, false);
            _heartUI[i] = temp;
            _heart_F[i] = temp.transform.Find("Heart_F").GetComponent<Image>();

        }
    } 
    void UpdateHUD()
    {
        setHeartUI();
        setHeartFill();
    }
   
}
