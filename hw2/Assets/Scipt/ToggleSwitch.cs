using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitch : MonoBehaviour
{
    public GameObject Box;
    public Material Steve;
    public Material Grass;

    private void Update()
    {
        if(Input.GetKey(KeyCode.E))
        {
            Debug.Log("E");
            Material Steve = Resources.Load("Steve", typeof(Material)) as Material;
            Box.GetComponent<Renderer>().material = Steve;
        }
        else
        {
            Material Grass = Resources.Load("Grass", typeof(Material)) as Material;
            Box.GetComponent<Renderer>().material = Grass;
        }
    }
}
