using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraController : Singleton<CameraController>
{
    // Start is called before the first frame update
    [Header("controller for lerping the Y damping during player Jump/fall")]
    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCamera;
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallYPanTime = 0.35f;
    [SerializeField] private float _normYPanAmount = 2;
    public float _fallSpeedYDampingChangeThreshold = -15f;

    public bool IsLerpingYDamping;
    public bool LerpedFromPlayerFalling;
    private Coroutine _lerpYPanCouroutine;

    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;
    

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < _allVirtualCamera.Length; i++)
        {
            if (_allVirtualCamera[i].enabled)
            {
                _currentCamera = _allVirtualCamera[i];
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }
        _normYPanAmount = 2;


    }

    #region Lerp the Y Damping

    public void LerpYDamping(bool isPlayerFalling)
    {
        _lerpYPanCouroutine = StartCoroutine(LerpYAction(isPlayerFalling));

        
    }
    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;
        float startDampAmount = _framingTransposer.m_YDamping;
        float endDampAmount = 0f;
        
        if (isPlayerFalling)
        {
            endDampAmount = _fallPanAmount;

            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = _normYPanAmount;
        }
        float elapsedTime = 0f;
        while(elapsedTime < _fallYPanTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime/_fallYPanTime) );
            _framingTransposer.m_YDamping = lerpedPanAmount;
            yield return null;
        }

        IsLerpingYDamping = false;
    }


    #endregion

}
