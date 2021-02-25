using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitch : MonoBehaviour
{
    public GameObject Box;
    public Material Steve;
    public Material Grass;
    public GameObject uiObject;

    void OnTriggerStay(Collider plyr)
    {
        if(plyr.tag == "Player" && Input.GetKey(KeyCode.G))
        {
            Debug.Log("E");
            Material Steve = Resources.Load("Materials/Steve", typeof(Material)) as Material;
            Box.GetComponent<Renderer>().material = Steve;
        }
        else
        {
            Material Grass = Resources.Load("Materials/Grass", typeof(Material)) as Material;
            Box.GetComponent<Renderer>().material = Grass;
        }
    }
    private void Start()
    {
        uiObject.SetActive(false);   
    }
    private void OnTriggerEnter(Collider player)
    {
        if(player.gameObject.tag == "Player")
        {
            uiObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider player)
    {
        if(player.gameObject.tag == "Player")
        {
            uiObject.SetActive(false);
        }
    }
}
