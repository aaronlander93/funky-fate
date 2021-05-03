/*
Code By: ???
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

    // All key game objects needed to unlock this door
    [SerializeField]
    private GameObject[] keyObjects;

    // For each key there needs to be a separate door sprite to represent that unlocked stage
    // The first sprite in the array is the door in the unlocked stage and every additional entry
    // should have one additional lock on the sprite
    [SerializeField]
    private Sprite[] doorSprites;
    public GameObject uiObject;
    public GameObject musicDie;
    private int numLocks;
    public string scene;

    void Start()
    {
        // uiObject.SetActive(false);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        numLocks = keyObjects.Length;

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
            foreach (GameObject key in keyObjects)
            {
                if (collision.gameObject == key)
                {
                    Destroy(collision.gameObject); // destroy the key
                    numLocks--;
                    spriteRenderer.sprite = doorSprites[numLocks];
                }
            }
        }

    }
    void OnTriggerStay2D(Collider2D plyr)
    {
        if(plyr.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.W))
        {
            //  && numLocks <= 0
            DestroyImmediate(musicDie);
            Destroy(uiObject);
            SceneManager.LoadScene(scene);
            Debug.Log("loading Scene");
        }
        if(plyr.tag == "Player" && numLocks == 0)
        {
            uiObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D plyr){
        if(plyr.tag == "Player")
        {
        uiObject.SetActive(false);
        }
    }
}
