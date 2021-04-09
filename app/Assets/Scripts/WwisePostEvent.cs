using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwisePostEvent : MonoBehaviour
{
    public AK.Wwise.Event PostEvent;
    // Start is called before the first frame update
    void JumpSound()
    {
        PostEvent.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
