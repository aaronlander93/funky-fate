using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundSensor : MonoBehaviour
{
    private bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.parent.tag == "ground")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.parent.tag == "ground")
        {
            isGrounded = false;
        }
    }

    public bool getState() { return isGrounded; }
}
