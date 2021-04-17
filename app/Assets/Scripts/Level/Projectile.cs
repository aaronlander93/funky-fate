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
    public float yOffset;
    public float flightTime;

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
        _rb.velocity = new Vector2(direction.x, direction.y + yOffset);
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
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if(collision.gameObject.tag =="Player")
        {
            Debug.Log("Hit");
            //damage player
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(1);
            Destroy(gameObject);
        }
        // else if (collision.gameObject.tag == "ground")
        // {
        //     Debug.Log("Hit the ground");
        //     Destroy(gameObject);
        // }
        else if (screenPos.y > Screen.height || screenPos.y < 0)
        {
            Debug.Log("Left cam");
            Destroy(gameObject);
        }
        else if (flightTime < 0)
        {
            Debug.Log("Hit the ground");
            Destroy(gameObject);
        }
        else
        {
            flightTime -= Time.deltaTime;
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
