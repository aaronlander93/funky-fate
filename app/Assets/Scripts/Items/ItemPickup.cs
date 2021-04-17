using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isPickedUp = false;
    private bool isFollowing = false;
    private GameObject player; // Player who picked up the item

    void OnCollisionEnter2D(Collision2D collision)
    {
        // When a player picks up an item
        // !!! Sound effect goes here !!!
        if (collision.gameObject.tag == "Player" && !isPickedUp)
        {
            this.player = collision.gameObject;
            isPickedUp = true;
            PickupItem(player);
        }
    }

    // Initially kicks the item up in the air and then coroutine starts player follow
    public void PickupItem(GameObject player)
    {
        rb = (Rigidbody2D)gameObject.AddComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(0, 12.0f), ForceMode2D.Impulse); // Kick item up in air
        rb.gravityScale = 2.0f; // Heavy gravity to come back down faster
        StartCoroutine(StartFollow());
    }

    // Follow the player
    IEnumerator StartFollow()
    {
        yield return new WaitForSeconds(0.7f); // Allow time for item to fly up
        gameObject.layer = 11; // Switch to physics layer that does not interact with other objects
        rb.gravityScale = 0.0f; // No gravity
        isFollowing = true;
    }

    void FixedUpdate()
    {
        if (isFollowing)
        {
            rb.AddForce((player.transform.position - transform.position) * 15.0f); // Add force towards player
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, 5.0f); // Limit velocity of floating item
            transform.Rotate(Vector3.forward * -2.0f); // Slowly rotate item
        }
    }
}