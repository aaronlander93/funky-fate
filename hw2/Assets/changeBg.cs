using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeBg : MonoBehaviour
{
    //Set these Textures in the Inspector
    public Texture m_bg1, m_bg2;
    Renderer m_Renderer;

    // Use this for initialization
    void Start () {
        //Fetch the Renderer from the GameObject
        m_Renderer = GetComponent<Renderer> ();

        //Set the Texture you assign in the Inspector as the first bg
        m_Renderer.material.SetTexture("m_bg1", m_bg1);
        //Set the Texture you assign in the Inspector as the second bg
        m_Renderer.material.SetTexture("m_bg2", m_bg2);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
