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

    [Header("JumpAttack")]
    public float horizontalForce = 5.0f;
    public float jumpForce = 10.0f;
    public bool groundPound = true;
    private bool cameraShake;
    private bool isGrounded = true;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector2 boxSize;

    public float aggroRange;

    [Header("Attacks")]
    public float cycleCooldown = 3f;    //time between attack cycles
    public float attCooldown = 2f;      //time between attacks in a cycle

    [Header("CowardlyPhase")]

    private bool isAggro = false;

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

        if (isAggro)
        {
            if(cowardPhase)
            {
                Retreat();
            }
            else
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

                bool landCheck = isGrounded; // Used to check if boss landed on ground
                isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
                _anim.SetBool("grounded", isGrounded);

                if (isGrounded)
                {
                    groundPound = true;
                    JumpTowardsPlayer();
                }
                // boss drops on player
                else if (Math.Abs(xDist) < 0.1f && groundPound)
                {
                    groundPound = false;
                    rb.velocity = Vector2.zero;
                    // rb.AddForce(new Vector2(0, -jumpForce), ForceMode2D.Impulse);
                }
                
            }
        }
        else if(Math.Abs(xDist) < aggroRange)
        {
            // Boss is now aggro'd and will always be aggro'd until death
            isAggro = true;
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //damage player if they come in contact with boss
        if(collision.gameObject.tag =="Player")
        {
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(1);
        }
    }
}
