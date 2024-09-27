using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class VFXManager : Singleton<VFXManager>
{
    // Start is called before the first frame update
    public CinemachineImpulseSource impulseSource;
    [SerializeField] private float _shockWaveTime = 0.1f;
    private Material _material;
    private Coroutine _shockWaveCoroutine;
    private static int _waveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");
    protected override void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
     /*   if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            impulseSource.GenerateImpulse();
        }*/
    }
    public void CallShockWave()
    {
        _shockWaveCoroutine = StartCoroutine(ShockWaveAction(-0.1f,1.2f));   
    }
    private IEnumerator ShockWaveAction(float startPos,float endPos)
    {
        _material.SetFloat(_waveDistanceFromCenter, startPos);
        float lerpedAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime<_shockWaveTime)
        {
            elapsedTime += Time.deltaTime;
            lerpedAmount = Mathf.Lerp(startPos, endPos, (elapsedTime/_shockWaveTime));   
            _material.SetFloat(_waveDistanceFromCenter,lerpedAmount);
            yield return null;
        }
    }
}
