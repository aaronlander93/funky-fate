using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class f_charAnim : MonoBehaviour
{
    public GameObject NPC;

    void Update()
    {
        if (Input.GetButtonDown("f_anim"))
        {
            NPC.GetComponent<Animator>().Play("dance");   
        }
        if (Input.GetButtonDown("f_animStop"))
        {
            NPC.GetComponent<Animator>().Play("stock");   
        }
    }
}
