using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    
    void Update()
    {
        if (Input.GetButtonDown("f.dance"))
        {
            NPC.GetComponent<Animator>().Play("here");   
        }
    }
}
