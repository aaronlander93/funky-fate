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
    private float aimlessDist;

    private Vector3 aimlessDirection = new Vector3(1, 0, 0);

    private Rigidbody2D rb;

    private System.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        gsc = GameObject.Find("GameSetupController").GetComponent<GameSetupController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        rand = new System.Random();
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
            WalkTowardsPlayer();
        }
        else if (idle)
        {
            Idle();
        }
        else
        {
            WalkAimlessly();
        }
    }

    private void AttackPlayer()
    {
        // Do nothing for now
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
            idleTime = rand.Next(120, 300);

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
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            gameObject.transform.position = new Vector2(currX + speed, currY);
        }
        else
        {
            // Walk to the left
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            gameObject.transform.position = new Vector2(currX - speed, currY);
        }

    }
}
