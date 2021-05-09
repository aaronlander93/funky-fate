/*
Code By: Aaron Lander & Milo Abril

This script controls the boss' decision making. 

There are two main phases: a cowardly phase and an attacking phase. 

During the cowardly phase, the boss simply runs away from the player, 
and is vulnerable to attacks. 

During the attack phase, the boss is invulnerable, and performs a combination
of jump, melee, and ranged attacks to harm the player.

*/


using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Photon.Pun;
using Photon.Realtime;

public class BossAI : MonoBehaviour
{
    public GameSetupController gsc;

    [Header("GroundedCheck")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector2 boxSize;

    [Header("JumpAttack")]
    public float jumpForce = 10.0f;
    public bool groundPound = true;
    private bool isGrounded = false;
    private float lastPos;
    private Vector2 jumpTo;
    private int numOfJumps;
    private CameraShake cameraShake;
    private bool isShook = false;
    private float shakeTime = 2f;

    [Header("Attacks")]
    private bool hasGuitar = true;
    private int lastAtt;
    public float initcycleCooldown = 4f;
    public int initNumOfAttsInCycle = 6;
    private int numOfAttsInCycle;
    public float initattCooldown = 1.5f;
    private float attCooldown = 4f;      //time between attacks in a cycle

    [Header("CowardlyPhase")]
    public float speed;
    private float aimlessDist;
    private bool idle = true;
    private float idleTime;

    // used to determine players in vicinity
    private Rigidbody2D closestPlayer;
    private float closestDist;
    private float xDist;

    //used to shake camera
    private CameraController Camera;

    private Rigidbody2D rb;

    public GameObject projectile;
    private Animator _anim;

    // Boss Phases
    private int attPhaseCounter = 3;    //number of attack cycles before switching to damage phase
    private bool cowardPhase = false;

    // Start is called before the first frame update
    void Start()
    {
        gsc = GameObject.Find("GameSetupController").GetComponent<GameSetupController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        
        gameObject.transform.localScale = new Vector3(-1, 1, 1);

        _anim = GetComponent<Animator>();
        _anim.SetFloat("AirSpeedY", -1f);
    }

    void FixedUpdate()
    {
        FindNearestPlayer();
        _anim.SetFloat("AirSpeedY", rb.velocity.y);

        if(cowardPhase)
        {
            dmgPhase();
        }
        else if (hasGuitar)
        {
            bool landCheck = isGrounded;
            isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
            
            _anim.SetBool("grounded", isGrounded);

            // If these two are different values, we know that the boss just landed on the ground.
            if(!landCheck && isGrounded)
            {
                cameraShake.ShakeCamera();
                isShook = true;
                shakeTime = 1.5f;
            }

            if(isShook && shakeTime < 0)
            {
                cameraShake.StopShake();
            }
            else
            {
                shakeTime -= Time.deltaTime;
            }

            if (isGrounded)
            {
                groundPound = true;
                lastPos = closestPlayer.transform.position.x;

                turnToPlayer();

                // cycle through to attacks
                Attack();
            }
            // boss drops on player's last position before leaving the ground
            else if (Math.Abs(transform.position.x - lastPos) < 0.1f && groundPound)
            {
                groundPound = false;
                rb.velocity = Vector2.zero;
                // can be used to drop over player faster
                rb.AddForce(new Vector2(0, -jumpForce/5), ForceMode2D.Impulse);
            }
        }
    }

    private void FindNearestPlayer()
    {
        List<Rigidbody2D> players = gsc.GetPlayers();
        
        closestDist = Mathf.Infinity;
        xDist = Mathf.Infinity;

        foreach (Rigidbody2D player in players)
        {
            float dist = Vector2.Distance(transform.position, player.transform.position);

            if (dist < closestDist)
            {
                closestDist = dist;
                closestPlayer = player;
                xDist = rb.position.x - player.transform.position.x;
            }
        }
    }

    private void Attack()
    {
        if (attCooldown > 0)
        {
            attCooldown -= Time.deltaTime;
            numOfAttsInCycle = (numOfAttsInCycle <= 0) ? initNumOfAttsInCycle : numOfAttsInCycle;
        }
        else
        {
            // logic to choose attack
            if (numOfJumps > 0)
            {
                // JumpTowardsPlayer();
                _anim.SetTrigger("jump");
                numOfJumps--;
            }
            else
            {
                int attack = UnityEngine.Random.Range(1, 4);
                while (attack == lastAtt)   // get another case until new attack isn't the same as the last one
                {
                    attack = UnityEngine.Random.Range(1, 4);
                }
                switch (attack)
                {
                    case 1:
                        // JumpTowardsPlayer();
                        _anim.SetTrigger("jump");
                        numOfJumps = UnityEngine.Random.Range(1, 3);    //number of jump attacks the boss will make
                        break;
                    case 2:
                        Attack1();
                        break;
                    case 3:
                        Attack3();
                        break;
                }
                lastAtt = attack;
                numOfAttsInCycle--;
                attPhaseCounter--;
            }

            attCooldown = (numOfAttsInCycle <= 0) ? initcycleCooldown : initattCooldown;
            
            if (attPhaseCounter < 0)
            {
                cowardPhase = true;
                _anim.SetTrigger("damagePhase");
            }
        }
    }

    private void Attack1()
    {
        // Debug.Log("Attack1");
        _anim.SetTrigger("attack1");
    }

    private void TomatoRain()
    {

    }

    private void Attack2()  //Probably not going to be used; may be used as boss intro
    {

    }

    private void Attack3()
    {
        // Debug.Log("Attack3");
        _anim.SetTrigger("attack3");
        hasGuitar = false;
    }

    private void ThrowProjectile()
    {
        if (!GameConfig.Multiplayer)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Hazards", "Tomato"), transform.position, Quaternion.identity);
        }
    }

    private void CatchProjectile()
    {
        _anim.SetTrigger("attack3p2");
        hasGuitar = true;
    }

    private void dmgPhase()
    {
        if (idle)
        {
            
            Idle();
        }
        else
        {
            
            RunAimlessly();
        }
    }

    private void Idle()
    {
        if(idleTime == 0)
        {
            idleTime = UnityEngine.Random.Range(50, 200); // * randHandler;

            idle = false;
        }
        else
        {
            idleTime -= 1;
        }
    }

    private void RunAimlessly()
    {
        
    }

    private void JumpTowardsPlayer()
    {
        rb.AddForce(new Vector2((-xDist * 7), jumpForce), ForceMode2D.Impulse);
    }

    private void turnToPlayer()
    {
        // Turn towards player
        if (xDist < 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
