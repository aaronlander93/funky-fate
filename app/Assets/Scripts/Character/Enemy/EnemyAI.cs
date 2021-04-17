using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameSetupController gsc;

    public float speed;
    public float attackRange;
    public float retreatRange;
    public float aggroRange;
    private float idleTime;
    //time between attacks
    public float initCooldownTime;
    private float attCooldown;
    
    private bool idle = true;

    private Rigidbody2D closestPlayer;
    private float closestDist;
    private float xDist;

    private float aimlessDist = 2;

    private Rigidbody2D rb;

    private System.Random rand;


    public GameObject projectile;
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        gsc = GameObject.Find("GameSetupController").GetComponent<GameSetupController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        rand = new System.Random();
        
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        FindNearestPlayer();

        if (Math.Abs(xDist) < aggroRange)
        {
            Debug.Log(xDist);
            //face player in range
            if (xDist < 0)
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }

            //attack or approach player depending on distance
            if (Math.Abs(xDist) > attackRange)
            {
                // Debug.Log("tracking player");
                _anim.SetBool("isWalking", true);
                WalkTowardsPlayer();
            }
            else if (Math.Abs(xDist) < retreatRange)
            {
                // Debug.Log("running from player");
                _anim.SetBool("isWalking", true);
                Retreat();
            }
            else
            {
                // Debug.Log(attCooldown);
                _anim.SetBool("isWalking", false);
                AttackPlayer();
            }
        }
        else if (idle)
        {
            _anim.SetBool("isWalking", false);
            Idle();
        }
        else
        {
            _anim.SetBool("isWalking", true);
            WalkAimlessly();
        }
    }

    private void FindNearestPlayer()
    {
        List<Rigidbody2D> players = gsc.GetPlayers();

        closestDist = Mathf.Infinity;
        xDist = Mathf.Infinity;

        foreach (Rigidbody2D player in players)
        {
            // dist = rb.position.x - player.transform.position.x;
            float dist = Vector2.Distance(transform.position, player.transform.position);

            if (dist < closestDist)
            {
                closestDist = dist;
                closestPlayer = player;
                xDist = rb.position.x - player.transform.position.x;
            }
        }
    }

    private void AttackPlayer()
    {
        if (attCooldown > 0)
        {
            // Debug.Log("Going to attack!");
            attCooldown -= Time.deltaTime;
        }
        else
        {
            // Debug.Log("attacking!");
            _anim.SetTrigger("attack"); //throwTomato() on animation event
            attCooldown = initCooldownTime;
        }
    }

    private void throwTomato()
    {
        Debug.Log("tomato thrown!");
        Instantiate(projectile, transform.position, Quaternion.identity);
    }

    private void WalkTowardsPlayer()
    {
        if (xDist < 0)
        {
            // Walk to the left
            Vector2 target = new Vector2(closestPlayer.position.x, rb.position.y);
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            // Walk to the right
            Vector2 target = new Vector2(-closestPlayer.position.x, rb.position.y);
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
    }

    private void Retreat()
    {
        if (xDist < 0)
        {
            // Walk to the right
            Vector2 target = new Vector2(-xDist, rb.position.y);
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            // Walk to the left
            Vector2 target = new Vector2(xDist, rb.position.y);
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
    }

    private void Idle()
    {
        if(idleTime == 0)
        {
            idleTime = rand.Next(50, 200);

            idle = false;
        }
        else
        {
            idleTime -= 1;
        }
    }

    private void WalkAimlessly()
    {
        if (aimlessDist < .3f && aimlessDist > -.3f)
        {
            // Determine distance
            aimlessDist = (float)rand.Next(-2, 3);
            idle = true;
        }
        else
        {
            // Move
            if (aimlessDist < 0)
            {
                // Walk to the left
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                // Walk to the right
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            Vector2 target = new Vector2(aimlessDist, rb.position.y);
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
    }
}
