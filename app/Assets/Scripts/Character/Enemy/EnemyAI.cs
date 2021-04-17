using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameSetupController gsc;

    public float attackRange;
    public float aggroRange;
    public float speed;
    

    private bool idle = true;

    private int idleTime = 0;

    private Rigidbody2D closestPlayer;
    private float closestDist;
    private float aimlessDist = 2;

    private Rigidbody2D rb;

    private System.Random rand;

    //time between attacks
    public float initCooldownTime;
    private float attCooldown;

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

        if(Math.Abs(closestDist) < attackRange)
        {
            AttackPlayer();
        }
        else if (Math.Abs(closestDist) < aggroRange)
        {
            //face player in range
            if (closestDist < 0)
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }

            //attack or approach player depending on distance
            if (Math.Abs(closestDist) < 5)
            {
                // Debug.Log(attCooldown);
                _anim.SetBool("isWalking", false);
                AttackPlayer();
            }
            else
            {
                // Debug.Log("tracking player");
                _anim.SetBool("isWalking", true);
                WalkTowardsPlayer();
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

    private void FindNearestPlayer()
    {
        List<Rigidbody2D> players = gsc.GetPlayers();

        float dist;
        closestDist = float.MaxValue;

        foreach (Rigidbody2D player in players)
        {
            dist = rb.position.x - player.transform.position.x;

            if (Math.Abs(dist) < Math.Abs(closestDist))
            {
                closestDist = dist;
                closestPlayer = player;
            }
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
            float currX = rb.position.x;
            float currY = rb.position.y;

            // Move
            if (aimlessDist < 0)
            {
                // Walk to the left
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
                gameObject.transform.position = new Vector2(currX - speed, currY);
                aimlessDist += speed;
            }
            else
            {
                // Walk to the right
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                gameObject.transform.position = new Vector2(currX + speed, currY);
                aimlessDist -= speed;
            }
        }
    }

    private void WalkTowardsPlayer()
    {
        float currX = rb.position.x;
        float currY = rb.position.y;

        if (closestDist < 0)
        {
            // Walk to the right
            gameObject.transform.position = new Vector2(currX + speed, currY);
        }
        else
        {
            // Walk to the left
            gameObject.transform.position = new Vector2(currX - speed, currY);
        }

    }
}
