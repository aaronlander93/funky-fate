using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform camTransform = default;
    private Vector2 _ogPosition = default;
    public float shakeFrequency = default;
    private bool isShaking = false;

    // Start is called before the first frame update
    void Start()
    {
        _ogPosition = camTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isShaking)
        {
            _ogPosition = camTransform.position;
        }
    }

    public void ShakeCamera()
    {
        camTransform.position = _ogPosition + Random.insideUnitCircle * shakeFrequency;
    }

    public void StopShake()
    {
        camTransform.position = _ogPosition;
    }
}
