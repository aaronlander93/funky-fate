using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameSetupController gsc;

    private GameObject closestPlayer;

    private bool walkingAimlessly = false;
    private bool idle = true;

    private int idleTime;

    private float closestDist;
    private float speed = .01f;
    private float aimlessDist = 0f;

    private System.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        gsc = GameObject.Find("GameSetupController").GetComponent<GameSetupController>();
        rand = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        FindNearestPlayer();

        if(Math.Abs(closestDist) < 5)
        {
            AttackPlayer();
        }
        else if (Math.Abs(closestDist) < 8)
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
        List<GameObject> players = gsc.GetPlayers();

        float dist;
        closestDist = float.MaxValue;


        foreach (GameObject player in players)
        {
            dist = gameObject.transform.position.x - player.transform.position.x;
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
            walkingAimlessly = true;
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
            
            aimlessDist = (float) rand.Next(-2, 3);

            walkingAimlessly = false;
            idle = true;
        }
        else
        {
            float currX = gameObject.transform.position.x;
            float currY = gameObject.transform.position.y;

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
        float currX = gameObject.transform.position.x;
        float currY = gameObject.transform.position.y;

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
