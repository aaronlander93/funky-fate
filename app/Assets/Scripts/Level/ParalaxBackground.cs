using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author:      Niall
// Purpose:     Move background objects based off camera position to create parallax effect
// Stolen from: https://www.youtube.com/watch?v=wBol2xzxCOU
// Changelog:   3/29/21 - created

public class ParalaxBackground : MonoBehaviour {

    public float paralaxMultiplier = .5f;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    void Start() {

    }

    void LateUpdate() {
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
            lastCameraPosition = cameraTransform.position;
        }
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += deltaMovement * paralaxMultiplier;
        lastCameraPosition = cameraTransform.position;

    }
}
