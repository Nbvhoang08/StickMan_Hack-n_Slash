using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class PlayBtn : MonoBehaviour
{
   private CinemachineImpulseSource _ImpulseSource;
    private void Start()
    {
        
        _ImpulseSource = GetComponent<CinemachineImpulseSource>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hitbox"))
        {
            ScreenShakeManager.Instance.CameraShake(_ImpulseSource);
        }
    }
}
