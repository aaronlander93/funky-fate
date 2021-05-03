/*
Code By: Andrew Sha
Code for Respawning the main character

Must add an empty game object in the game and put the respawn code on said gameobject
make sure you add some animations and sprites :)
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("CheckPoint");
        gameManager = FindObjectOfType<GameManager>();
    }

    public void RespawnPlayer ()
    {
        StartCoroutine("RespawnPlayer");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            gameManager.currentCheckpoint = gameObject;
            Debug.Log("Activated CheckPoint!" + transform.position);
        }
    }
}
