/*
Code By: Andrew Sha
This code makes the camera follow a 2D entity around.

Edited by: Niall Healy
Added camera smoothing based off this tutorial:
https://www.youtube.com/watch?v=MFQhpwc6cKE

Edited by: Joseph Babel
Add camera lock state and function to smoothly reposition camera to new fixed position
with different zoom level.
*/

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera orthographicCamera;
    public PhotonView photonView;

    public Movement2D player;
    public float smoothSpeed = 0.125f;

    public float xOffSet;
    public float yOffSet;

    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;
    
    private bool cameraLocked = false;
    private Vector3 cameraLockedPosition;

    private float defaultZoom;
    private float newZoom;
    private float smoothedZoom;

    // Start is called before the first frame update
    void Start()
    {
        this.orthographicCamera = gameObject.GetComponent<Camera>();
        this.defaultZoom = orthographicCamera.orthographicSize;

        if (photonView && !photonView.IsMine)
        {
            //Destroy(gameObject.GetComponent<CameraController>());
            Destroy(gameObject);
        }
        else if(photonView && photonView.IsMine)
        {
            this.player = photonView.gameObject.GetComponent<Movement2D>();
        }
    }

    // Using FixedUpdate here instead of Update to ensure no race condition between
    // player and camera movement
    void FixedUpdate()
    {
        if (player)
        {
            if (cameraLocked)
            {
                this.desiredPosition = new Vector3(cameraLockedPosition.x, cameraLockedPosition.y, cameraLockedPosition.z);
                this.smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed / 2);
                transform.position = smoothedPosition;

                this.smoothedZoom = Mathf.Lerp(orthographicCamera.orthographicSize, newZoom, smoothSpeed / 2);
                orthographicCamera.orthographicSize = smoothedZoom;
            }
            else
            {
                this.desiredPosition = new Vector3(player.transform.position.x + xOffSet, player.transform.position.y + yOffSet, transform.position.z);
                this.smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
                transform.position = smoothedPosition;

                this.smoothedZoom = Mathf.Lerp(orthographicCamera.orthographicSize, defaultZoom, smoothSpeed / 2);
                orthographicCamera.orthographicSize = smoothedZoom;
            }
        }
    }

    public void LockCameraToPosition(Vector3 position, float zoomLevel)
    {
        this.cameraLocked = true;
        this.cameraLockedPosition = position;
        this.newZoom = zoomLevel;
    }

    public void UnlockCamera()
    {
        this.cameraLocked = false;
    }
}
