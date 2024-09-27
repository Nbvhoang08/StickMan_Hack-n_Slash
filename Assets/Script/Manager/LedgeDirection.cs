using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
public class LedgeDirection : MonoBehaviour
{
    public CustomInspectorObject customInspectorObject;
    private Collider2D _coll;

    private void Start()
    {
        _coll = GetComponent<Collider2D>(); 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (customInspectorObject.panCameraOnContact)
            {
                CameraController.Instance.PanCameraOnContact(customInspectorObject.panDistance,
                    customInspectorObject.panTime, customInspectorObject.panDirection, false);
                Debug.Log("down");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            /*Vector2 exitDirection = (collision.transform.position - _coll.bounds.center).normalized;
            if (customInspectorObject.swapCameras && customInspectorObject.cameraOnLeft !=null && customInspectorObject.cameraOnRight!= null)
            {
                CameraController.Instance.SwapCamera(customInspectorObject.cameraOnLeft,customInspectorObject.cameraOnRight,exitDirection);
            }*/
            if (customInspectorObject.panCameraOnContact)
            {
                CameraController.Instance.PanCameraOnContact(customInspectorObject.panDistance,
                    customInspectorObject.panTime, customInspectorObject.panDirection, true);
                Debug.Log("up");
            }
        }
    }
}


[System.Serializable]
public class CustomInspectorObject
{
    public bool swapCameras = false;
    public bool panCameraOnContact = false;

    [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;
    [HideInInspector] public CinemachineVirtualCamera cameraOnRight;
    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
}


[CustomEditor(typeof(LedgeDirection))]
public class MyScriptEditor : Editor
{
    LedgeDirection ledgeDirection;

    private void OnEnable()
    {
        ledgeDirection = (LedgeDirection)target; 
    }
    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector();
        if(ledgeDirection.customInspectorObject.swapCameras)
        {
            // Sử dụng EditorGUILayout.ObjectField thay vì EditorGUI.ObjectField
            ledgeDirection.customInspectorObject.cameraOnLeft = (CinemachineVirtualCamera)EditorGUILayout.ObjectField(
                "Camera on left",
                ledgeDirection.customInspectorObject.cameraOnLeft,
                typeof(CinemachineVirtualCamera),
                true);
            // Sử dụng EditorGUILayout.ObjectField thay vì EditorGUI.ObjectField
            ledgeDirection.customInspectorObject.cameraOnRight = (CinemachineVirtualCamera)EditorGUILayout.ObjectField(
                "Camera on right",
                ledgeDirection.customInspectorObject.cameraOnRight,
                typeof(CinemachineVirtualCamera),
                true);
        }
        if(ledgeDirection.customInspectorObject.panCameraOnContact)
        {
            ledgeDirection.customInspectorObject.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Camera on Direction",
                ledgeDirection.customInspectorObject.panDirection);
            ledgeDirection.customInspectorObject.panDistance = EditorGUILayout.FloatField("Pan Distance",ledgeDirection.customInspectorObject.panDistance);
            ledgeDirection.customInspectorObject.panTime = EditorGUILayout.FloatField("Pan Time", ledgeDirection.customInspectorObject.panTime);
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(ledgeDirection);
        }
    }
}
#endif
public enum PanDirection
{
    Up,
    Down,
    Left,
    Right
}


