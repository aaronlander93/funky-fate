using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameSetupController gsc;
    Rigidbody2D _rb;
    private Rigidbody2D closestPlayer;

    private Vector2 direction;
    public float moveSpeed;
    public float yOffset;
    public float lifeTime;
    public float distance;
    public LayerMask collidables;

    void Awake()
    {
        gsc = GameObject.Find("GameSetupController").GetComponent<GameSetupController>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
        FindNearestPlayer();
        
        direction = (closestPlayer.transform.position - transform.position).normalized * moveSpeed;
        _rb.velocity = new Vector2(direction.x, direction.y + yOffset);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FindNearestPlayer()
    {
        List<Rigidbody2D> players = gsc.GetPlayers();

        float closestDist = Mathf.Infinity;

        foreach (Rigidbody2D player in players)
        {
            // dist = rb.position.x - player.transform.position.x;
            float dist = Vector2.Distance(transform.position, player.transform.position);

            if (dist < closestDist)
            {
                closestDist = dist;
                closestPlayer = player;
            }
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
        else if (collision.transform.parent != null && collision.transform.parent.tag == "ground")
        {
            Debug.Log("Hit the ground");
            Destroy(gameObject);
        }
        else if (screenPos.y > Screen.height || screenPos.y < 0)
        {
            Debug.Log("Left cam");
            Destroy(gameObject);
        }
    }

    void DestroyProjectile() { Destroy(gameObject); }
}
