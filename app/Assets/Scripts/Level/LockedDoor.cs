using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private int numLocks;

    void Start()
    {
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

    void OnCollisionEnter2D(Collision2D collision)
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
}
