/*
Code By: Joseph Babel
This script unlocks a door with two locks
It will render a new image when the key gets used up

Worked on (Andrew Sha)
Added in scene loading when the locks are gone.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LockedDoor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // For each key there needs to be a separate door sprite to represent that unlocked stage
    // The first sprite in the array is the door in the unlocked stage and every additional entry
    // should have one additional lock on the sprite
    [SerializeField]
    private Sprite[] doorSprites;
    public GameObject promptText;
    public int numLocks;
    public string scene;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (numLocks == 0) // unlocked
        {
            spriteRenderer.sprite = doorSprites[0];
        }
        else // locked
        {
            spriteRenderer.sprite = doorSprites[numLocks];
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key" && numLocks > 0)
        {
            Destroy(collision.gameObject); // destroy the key
            numLocks--;
            spriteRenderer.sprite = doorSprites[numLocks];
        }

    }
    void OnTriggerStay2D(Collider2D plyr)
    {
        if(plyr.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.W) && numLocks <= 0)
        {
            Destroy(promptText);
            SceneManager.LoadScene(scene);
            Debug.Log("loading Scene");
        }
        if(plyr.tag == "Player" && numLocks == 0)
        {
            promptText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D plyr){
        if(plyr.tag == "Player")
        {
            promptText.SetActive(false);
        }
    }
}
