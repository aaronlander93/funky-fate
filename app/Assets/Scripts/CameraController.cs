/*
Code By: Andrew Sha
This code makes the camera follow a 2D entity around.

Edited by: Niall Healy
Added camera smoothing based off this tutorial:
https://www.youtube.com/watch?v=MFQhpwc6cKE
*/

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PhotonView photonView;

    public Movement2D player;
    public float smoothSpeed = 0.125f;

    public float xOffSet;
    public float yOffSet;

    // Start is called before the first frame update
    void Start()
    {
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

    void Update()
    {

    }

    // Using FixedUpdate here instead of Update to ensure no race condition between
    // player and camera movement
    void FixedUpdate()
    {
        if (player)
        {
            Vector3 desiredPosition = new Vector3(player.transform.position.x + xOffSet, player.transform.position.y + yOffSet, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
