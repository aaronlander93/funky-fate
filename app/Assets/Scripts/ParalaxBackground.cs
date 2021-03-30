using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxBackground : MonoBehaviour {

    public float paralaxMultiplier = .5f;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    void Start() {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate() {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += deltaMovement * paralaxMultiplier;
        lastCameraPosition = cameraTransform.position;

    }
}
