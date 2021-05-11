using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GameObject keyPrefab;
    public Rigidbody2D rb; // Enable rigidbody after picked up

    // On pickup kick item up into air
    [SerializeField]
    private float kickUpForce = 12.0f;
    [SerializeField]
    private float initialItemGravity = 2.0f;

    // After some seconds follow player
    [Space(10)]
    [SerializeField]
    private float secondsToFollow = 0.7f; 
    [Space(0)]
    [SerializeField]
    private float followForce = 15.0f;


    // Limit follow speed
    [Space(10)]
    [SerializeField]
    private float maxFollowVelocity = 5.0f;
    [Space(0)]

    // Rotate speed
    [SerializeField]
    private float rotateSpeed = -2.0f;

    private bool isPickedUp = false;
    private bool isFollowing = false;
    private GameObject player; // Player who picked up the item
    private int keyID;

    void Start()
    {
        rb.isKinematic = true;
    }

    public int KeyID
    {
        set { keyID = value; }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // When a player picks up an item
        // !!! Sound effect goes here !!!
        if (collision.gameObject.tag == "Player" && !isPickedUp)
        {
            this.player = collision.gameObject;
            // Subscribe to PlayerDeath event so key can be respawned when player dies
            player.GetComponentInChildren<CharacterHealth>().PlayerDeath += RespawnKey;
            isPickedUp = true;
            PickupItem(player);
        }
    }

    // Initially kicks the item up in the air and then coroutine starts player follow
    public void PickupItem(GameObject player)
    {
        rb.isKinematic = false;
        rb.AddForce(new Vector2(0, kickUpForce), ForceMode2D.Impulse); // Kick item up in air
        rb.gravityScale = initialItemGravity; // Heavy gravity to come back down faster
        StartCoroutine(StartFollow()); // Follow player
    }

    // Follow the player
    IEnumerator StartFollow()
    {
        yield return new WaitForSeconds(secondsToFollow); // Allow time for item to fly up
        gameObject.layer = 11; // Switch to physics layer that does not interact with other objects
        rb.gravityScale = 0.0f; // No gravity
        isFollowing = true;
    }

    void FixedUpdate()
    {
        if (isFollowing)
        {
            rb.AddForce((player.transform.position - transform.position) * followForce); // Add force towards player
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxFollowVelocity); // Limit velocity of floating item
            transform.Rotate(Vector3.forward * rotateSpeed); // Slowly rotate item
        }
    }

    void RespawnKey()
    {
        var key = Instantiate(keyPrefab, GetKeySpawn(), Quaternion.identity); // Create new key
        key.GetComponentInChildren<ItemPickup>().KeyID = keyID; // Set new key's id
        Destroy(gameObject); // Destroy this key
    }

    Vector2 GetKeySpawn()
    {
        Vector2 spawnLoc = default;

        switch(keyID)
        {
            case 0:
                spawnLoc = new Vector2(-28.297f, -4.439f);
                break;
            case 1:
                spawnLoc = new Vector2(19.8f, -22.83f);
                break;
        }

        return spawnLoc;
    }

    void OnDestroy()
    {
        if(player) player.GetComponentInChildren<CharacterHealth>().PlayerDeath -= RespawnKey;
    }
}