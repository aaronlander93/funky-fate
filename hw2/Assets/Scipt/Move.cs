using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Animator Dance;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            Debug.Log("Q");
            Dance.enabled = true;
        }
        else
        {
            Dance.enabled = false;
        }
    }
}
