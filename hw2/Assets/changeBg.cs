using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeBg : MonoBehaviour
{
    //Set these Textures in the Inspector
    public Material[] mats = new Material[2];
    public int curMat;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
                if (curMat == 0) {
                    GetComponent<Renderer>().material = mats[0];
                    curMat = 1;
                }
                else if (curMat == 1) {
                    GetComponent<Renderer>().material = mats[1];
                    curMat = 0;
                }
			}
		}
    }
}
