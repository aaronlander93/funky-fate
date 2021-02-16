using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCapsule : MonoBehaviour
{

    private float rotateSpeed = 2.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rotateSpeed += .5f;
        }

        transform.Rotate(rotateSpeed, 0, 0);
    }
}
