/*
Code By: Joseph Babel
Upon collision call function from camera to fix its position at the location
of attached game object. Preferably an empty game object with Box Collider 2D. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockZone : MonoBehaviour
{
    [SerializeField]
    private float zoomLevel = 4.5f; // new zoom level to transition to for larger areas

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Call LockCameraToPosition from CameraController of player who entered this zone
            collision.gameObject.transform.parent.Find("Main Camera").GetComponent<CameraController>().LockCameraToPosition(this.gameObject.transform.position, zoomLevel);
        }
    }

    
}
