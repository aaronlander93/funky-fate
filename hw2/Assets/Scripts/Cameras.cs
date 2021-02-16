using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameras : MonoBehaviour
{
    public Camera[] _cameras;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _cameras.Length; i++)
        {
            if(i == 0)
            {
                _cameras[i].enabled = true;
            }
            else
            {
                _cameras[i].enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach(Camera cam in _cameras)
            {
                cam.enabled = !cam.enabled;
            }
        }
    }
}
