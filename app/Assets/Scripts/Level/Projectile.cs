using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D _rb;
    private GameObject[] players;
    private GameObject closestPlayer;

    private Vector2 direction;
    public float moveSpeed;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        FindNearestPlayer();
        
        direction = (closestPlayer.transform.position - transform.position).normalized * moveSpeed;
        _rb.velocity = new Vector2(direction.x, direction.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(closestPlayer == null)
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag =="Player")
        {
            Debug.Log("Hit");
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "ground")
        {
            Debug.Log("Hit the ground");
            Destroy(gameObject);
        }
 
    }

    private void FindNearestPlayer()
    {
        float dist;
        float closestDist = float.MaxValue;

        foreach(GameObject player in players)
        {
            dist = gameObject.transform.position.x - player.transform.position.x;
            if (Math.Abs(dist) < Math.Abs(closestDist))
            {
                closestPlayer = player;
            }
        }
    }
}
