/*
Code By: Andrew Sha
This code makes the camera follow a 2D entity around.

Edited by: Niall Healy
Added camera smoothing based off this tutorial:
https://www.youtube.com/watch?v=MFQhpwc6cKE
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Movement2D player;
    public float smoothSpeed = 0.125f;
    public bool isFollowing;

    public float xOffSet;
    public float yOffSet;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Movement2D>();

        isFollowing = true;
    }

    // Using FixedUpdate here instead of Update to ensure no race condition between
    // player and camera movement
    void FixedUpdate()
    {
        if (isFollowing)
        {
            Vector3 desiredPosition = new Vector3(player.transform.position.x + xOffSet, player.transform.position.y + yOffSet, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        };
    }
}
