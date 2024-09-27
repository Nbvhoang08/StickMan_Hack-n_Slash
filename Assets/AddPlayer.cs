using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlayer : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    // Start is called before the first frame update

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();  
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
       
        }
       

    }
    void Start()
    {
        virtualCamera.Follow = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
