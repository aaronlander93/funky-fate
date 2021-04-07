using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject[] players;

    private GameObject closestPlayer;

    private bool walkingAimlessly = false;
    private bool idle = true;

    private int idleTime;

    private float closestDist;
    private float speed = .01f;
    private float aimlessDist = 0f;

    private System.Random rand;

    private Animator _anim;
    private bool _facingRight = false;

    // Start is called before the first frame update
    void Start()

    {
        players = GameObject.FindGameObjectsWithTag("Player");
        rand = new System.Random();
        
        _anim = GetComponent<Animator>();
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

    void Flip()
    {
        _facingRight = !_facingRight;
        
        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void AttackPlayer()
    {
        // Do nothing for now
    }

    private void FindNearestPlayer()
    {
        float dist;
        closestDist = float.MaxValue;

        foreach(GameObject player in players)
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

            _anim.SetBool("isIdle", false);
            _anim.SetBool("isWalking", true);
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
            
            _anim.SetBool("isIdle", true);
            _anim.SetBool("isWalking", false);
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

    void animate() 
    {
        
    }
}
