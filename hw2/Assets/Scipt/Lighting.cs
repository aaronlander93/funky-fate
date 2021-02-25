using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour
{

    public GameObject Light;
    // Update is called once per frame
    void Update()
    {
        int night = 150;
        int day = 50;

        if(Input.GetKey(KeyCode.F))
        {
            Debug.Log("F");
            Light.transform.eulerAngles = new Vector3(night, 0, 0);
        }
        else
        {
            Light.transform.eulerAngles = new Vector3(day, 0, 0);
        }
    }
}
