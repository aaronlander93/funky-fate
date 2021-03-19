/*
Code By: Aaron Lander
This script controls and keeps track of the character's health.
Whenever the character takes damage, a call is made to the HealthBar
object to change the current health bar image on the UI. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private int m_CharacterHealth = 7;
    private bool m_isDead = false;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        // For testing purposes. This needs to be removed eventually
        if (Input.GetKeyDown("f3"))
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        if(m_CharacterHealth > 0)
        {
            --m_CharacterHealth;
        }
        else
        {
            m_isDead = true;
        }
        
    }

    public int GetCharacterHealth()
    {
        return m_CharacterHealth;
    }
}
