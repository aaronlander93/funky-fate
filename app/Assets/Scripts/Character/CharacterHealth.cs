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
    [SerializeField] private int characterHealth = 5;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        characterHealth--;

        if(characterHealth == 0)
        {
            // Death animation will go here in the future
            Destroy(gameObject);
        }
    }

    public int GetCharacterHealth()
    {
        return characterHealth;
    }
}
