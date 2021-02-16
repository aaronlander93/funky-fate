using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeShade : MonoBehaviour
{
    //Set these Textures in the Inspector
    public Shader[] shade = new Shader[2];
    public int curShader;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
                if (curShader == 0) {
                    GetComponent<Renderer>().material.shader = shade[0];
                    curShader = 1;
                }
                else if (curShader == 1) {
                    GetComponent<Renderer>().material.shader = shade[1];
                    curShader = 0;
                }
			}
		}
    }
}
