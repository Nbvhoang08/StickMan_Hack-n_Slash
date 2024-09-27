using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using JetBrains.Annotations;

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
    private Coroutine _panCameraCouroutine;
    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;
    private Vector2 _startingTrackedObjectOffset;

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


    #region Pan camera
    public void PanCameraOnContact(float panDistance,float panTime,PanDirection panDirection,bool panToStartingPos)
    {
        _panCameraCouroutine = StartCoroutine(PanCamera(panDistance,panTime,panDirection,panToStartingPos));
    }

    private IEnumerator PanCamera(float panDistance,float panTime ,PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;
        if(!panToStartingPos)
        {
            switch(panDirection)
            {
                case PanDirection.Left:
                    endPos = Vector2.left;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.right;
                    break;
                case PanDirection.Up:
                    endPos = Vector2.up;
                     break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                default:
                    break;
            }
            endPos *= panDistance;
            startingPos = _startingTrackedObjectOffset;
            endPos += startingPos;

        }
        else
        {
            startingPos = _framingTransposer.m_TrackedObjectOffset;
            endPos = _startingTrackedObjectOffset;
        }
        float elapsedTime = 0f;
        while (elapsedTime< panTime) 
        { 
            elapsedTime += Time.deltaTime;
            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            _framingTransposer.m_TrackedObjectOffset = panLerp;
            yield return null;
        }
    }
    #endregion


    #region Swap camera
    public void SwapCamera(CinemachineVirtualCamera camFromLeft,CinemachineVirtualCamera camFromRight,Vector2 TriggerExitDirection)
    {
        if(_currentCamera == camFromRight && TriggerExitDirection.x < 0)
        {
            camFromRight.enabled = false;
            camFromLeft.enabled = true;
            _currentCamera = camFromRight;
            _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        }
        else if (_currentCamera == camFromLeft && TriggerExitDirection.x > 0)
        {
            camFromRight.enabled = true;
            camFromLeft.enabled = false;
            _currentCamera = camFromLeft;
            _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        }
    }
    #endregion

}
