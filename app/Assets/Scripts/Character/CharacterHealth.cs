/*
Code By: Aaron Lander
This script controls and keeps track of the character's health.
Whenever the character takes damage, a call is made to the HealthBar
object to change the current health bar image on the UI. 

Worked on (Andrew Sha)
Added in maxplayer health to give character max health when respawn
Added in GameManager variable to respawn character
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private int characterHealth = 5;
    public int maxPlayerHealth;
    public bool dead = false;
    // private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        maxPlayerHealth = characterHealth;
        // gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterHealth <= 0)
        {
            characterHealth = 0;
            // gameManager.RespawnPlayer();
            dead = true;
        }
    }

    public void TakeDamage(int dmgNum)
    {
        characterHealth-= dmgNum;

        Debug.Log("playerhealth = " + characterHealth);
        if(characterHealth <= 0)
        {
            // Death animation will go here in the future
            // gameManager.RespawnPlayer();
            // Destroy(gameObject);
        }
    }

    public int GetCharacterHealth()
    {
        return characterHealth;
    }

    public void FullHealth()
    {
        characterHealth = maxPlayerHealth;
    }
}
