using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class f_animControl : MonoBehaviour
{
    public GameObject NPC;

    void Update()
    {
        if (Input.GetButtonDown("f.dance"))
        {
            NPC.GetComponent<Animator>().Play("here");   
        }
    }
}
