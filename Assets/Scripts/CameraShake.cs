using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 _cameraPosition;
    [SerializeField]
    private float _shakeMagnitude = 0.05f;
    [SerializeField]
    private float _shakeTime = 0.5f;

    public void Shake()
    {
        _cameraPosition = transform.position;
        InvokeRepeating("StartShaking", 0f, 0.005f);
        Invoke("StopShaking", _shakeTime);
    }

    void StartShaking()
    {
        float cameraShakingX = Random.value * _shakeMagnitude * 2;
        float cameraShakingY = Random.value * _shakeMagnitude * 2;
        Vector3 cameraPosition = _cameraPosition;
        cameraPosition.x += cameraShakingX;
        cameraPosition.y += cameraShakingY;
        transform.position = cameraPosition;
    }

    void StopShaking()
    {
        CancelInvoke("StartShaking");
        transform.position = _cameraPosition;
    }
}
