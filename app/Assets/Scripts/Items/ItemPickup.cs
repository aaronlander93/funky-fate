using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isPickedUp = false;
    private bool isFollowing = false;
    private GameObject player;

    public void AnimateItemPickup(GameObject player)
    {
        if (isPickedUp == false)
        {
            rb = (Rigidbody2D)gameObject.AddComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(0, 10.0f), ForceMode2D.Impulse);
            isPickedUp = true;
            StartCoroutine(StartFollow());
        }

        this.player = player;
    }

    IEnumerator StartFollow()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.layer = 11;
        rb.gravityScale = 0.0f;
        isFollowing = true;
    }

    void FixedUpdate()
    {
        if (isFollowing)
        {
            rb.AddForce((player.transform.position - transform.position) * 15.0f);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, 5.0f);
        }
    }
}