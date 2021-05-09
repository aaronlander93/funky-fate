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
    private bool cameraShake;

    [Header("Attacks")]
    public float initcycleCooldown = 2f;
    public int initNumOfAttsInCycle = 6;
    private int numOfAttsInCycle;
    public float initattCooldown = 1f;
    private float attCooldown;      //time between attacks in a cycle

    [Header("CowardlyPhase")]


    // used to determine players in vicinity
    private Rigidbody2D closestPlayer;
    private float closestDist;
    private float xDist;

    //used to shake camera
    private CameraController Camera;

    private Rigidbody2D rb;
    private groundSensor gSensor;

    public GameObject projectile;
    private Animator _anim;

    // Boss Phases
    private bool cowardPhase = false;
    private bool meleePhase = false;
    private bool throwingPhase = false;

    // Start is called before the first frame update
    void Start()
    {
        gsc = GameObject.Find("GameSetupController").GetComponent<GameSetupController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        gSensor = gameObject.transform.GetChild(0).GetComponent<groundSensor>();

        _anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        FindNearestPlayer();

        if(cowardPhase)
        {
            Retreat();
        }
        else
        {
            isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
            
            // isGrounded = gSensor.getState();
            _anim.SetBool("grounded", isGrounded);

            if (isGrounded)
            {
                groundPound = true;
                lastPos = closestPlayer.transform.position.x;

                // Turn towards player
                if (xDist < 0)
                {
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    gameObject.transform.localScale = new Vector3(-1, 1, 1);
                }

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
                JumpTowardsPlayer();
                numOfJumps--;
            }
            else
            {
                int attack = UnityEngine.Random.Range(1, 4);
                switch (attack)
                {
                    case 1:
                        JumpTowardsPlayer();
                        numOfJumps = UnityEngine.Random.Range(0, 2);    //number of jump attacks the boss will make
                        break;
                    case 2:
                        Attack1();
                        break;
                    case 3:
                        Attack3();
                        break;
                }
                // Debug.Log(attack);
                numOfAttsInCycle--;
            }

            attCooldown = (numOfAttsInCycle <= 0) ? initcycleCooldown : initattCooldown;
        }
    }

    private void Attack1()
    {
        Debug.Log("Attack1");
    }

    private void TomatoRain()
    {

    }

    private void Attack2()  //Probably not going to be used; may be used as boss intro
    {

    }

    private void Attack3()
    {
        Debug.Log("Attack3");
    }

    private void ThrowProjectile()
    {

    }

    private void Retreat()
    {
        
    }

    private void JumpTowardsPlayer()
    {
        rb.AddForce(new Vector2((-xDist * 7), jumpForce), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //damage player if they come in contact with boss
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(1);
        }
    }
}
