/*
Code By: Aaron Lander
This script controls and keeps track of the character's health.
Whenever the character takes damage, a call is made to the HealthBar
object to change the current health bar image on the UI. 

Worked on (Andrew Sha)
Added in maxplayer health to give character max health when respawn
Added in GameManager variable to respawn character
*/

using Photon.Pun;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterHealth : MonoBehaviour
{
    public GameSetupController gsc;
    public GameObject player;
    public GameObject explosion;
    public int maxPlayerHealth;

    public delegate void DeathEvent();
    public event DeathEvent PlayerDeath = delegate { };

    private int characterHealth;

    // Start is called before the first frame update
    void Start()
    {
        gsc = GameObject.Find("GameSetupController").GetComponent<GameSetupController>();
        characterHealth = maxPlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (characterHealth <= 0)
        {
            characterHealth = 0;
        }
    }

    public void TakeDamage(int dmgNum)
    {
        // Prevents health from going below 0
        if(characterHealth > 0)
        {
            characterHealth -= dmgNum;

            // Player is dead
            if (characterHealth <= 0)
            {
                characterHealth = 0;
                TriggerDeath();
            }
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

    private void TriggerDeath()
    {
        if (!GameConfig.Multiplayer)
            Instantiate(explosion, transform.position, Quaternion.identity);
        else
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "FX", "Explosion"), transform.position, Quaternion.identity);
        
        StartCoroutine(Respawn());
        PlayerDeath();
    }

    IEnumerator Respawn()
    {
        // Turn off animation and disable movement
        player.GetComponentInChildren<SpriteRenderer>().enabled = false;
        player.GetComponentInChildren<Movement2D>().enabled = false;

        // Pause briefly after death to let player process what happened
        yield return new WaitForSeconds(2f);

        // Respawn player
        gsc.RespawnPlayer();

        // Turn animation and movement back on
        player.GetComponentInChildren<SpriteRenderer>().enabled = true;
        player.GetComponentInChildren<Movement2D>().enabled = true;
    }
}
