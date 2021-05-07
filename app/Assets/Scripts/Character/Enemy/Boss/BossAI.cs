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
    public float horizontalForce = 5.0f;
    public float jumpForce = 10.0f;
    public bool groundPound = true;
    public float jumpVelocity = 10.0f;
    private bool cameraShake;
    private bool isGrounded = false;
    private float lastPos;
    private Vector2 jumpTo;

    [Header("Attacks")]
    public float initcycleCooldown = 3f;
    private float cycleCooldown;    //time between attack cycles
    public float initattCooldown = 2.4f;
    private float attCooldown;      //time between attacks in a cycle

    [Header("CowardlyPhase")]


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
    private bool cowardPhase = false;
    private bool meleePhase = false;
    private bool throwingPhase = false;

    // Start is called before the first frame update
    void Start()
    {
        gsc = GameObject.Find("GameSetupController").GetComponent<GameSetupController>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        
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
            bool landCheck = isGrounded; // Used to check if boss landed on ground
            isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
            _anim.SetBool("grounded", isGrounded);

            if (isGrounded)
            {
                groundPound = true;
                lastPos = -xDist;

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
            // boss drops on player
            else if (/*transform.position.x == lastPos*/ Math.Abs(xDist) < 0.1f && groundPound)
            {
                Debug.Log(lastPos);
                groundPound = false;
                rb.velocity = Vector2.zero;
                // can be used to drop over player faster
                // rb.AddForce(new Vector2(0, -jumpForce), ForceMode2D.Impulse);
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
        }
        else
        {
            JumpTowardsPlayer();
            Attack1();
            Attack3();
            attCooldown = initattCooldown;
        }
    }

    private void Attack1()
    {

    }

    private void TomatoRain()
    {

    }

    private void Attack2()  //Probably not going to be used; may be used as boss intro
    {
        
    }

    private void Attack3()
    {
        
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
        // jumpTo = new Vector2(lastPos, 60);
        // transform.position = Vector2.MoveTowards(transform.position, jumpTo, jumpVelocity * Time.deltaTime);
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
