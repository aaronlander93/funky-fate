/*
Code By: Aaron Lander

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

public class BossAIv1 : MonoBehaviour
{
    public GameSetupController gsc;

    [Header("JumpAttack")]
    [SerializeField] float jumpHeight;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector2 boxSize;
    [SerializeField] int initJumpsLeft;  // # of jumps left before cowardly phase
    private int jumpsLeft;

    private bool isGrounded;

    [Header("MeleeAttack")]
    public float speed;
    public float attackRange;
    public float aggroRange;
    public int initAttackCooldown;
    public int initNumAttacks;
    private int attackCooldown;
    private int numAttacksLeft;

    [Header("Throwing Attacks")]
    public int initNumThrows;
    public float initThrowCooldown;
    public int initWalkTime;
    public float walkRange;
    private int numThrowsLeft;
    private float throwCooldown;

    [Header("CowardlyPhase")]
    public float retreatRange;
    public int initCowardlyTime;
    private int cowardlyTime;

    private bool isAggro = false;

    private Rigidbody2D closestPlayer;
    private float closestDist;
    private float xDist;

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

        numThrowsLeft = initNumThrows;
        throwCooldown = initThrowCooldown;
        numAttacksLeft = initNumAttacks;
        attackCooldown = initAttackCooldown;
        cowardlyTime = initCowardlyTime;
        jumpsLeft = initJumpsLeft;

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

                // If these two are different values, we know that the boss just landed on the ground.
                if(!landCheck && isGrounded)
                {
                    meleePhase = true;
                }

                // Make decision based on current phase
                if (isGrounded && meleePhase)
                {
                    MeleeAttack();
                }
                else if(isGrounded && throwingPhase)
                {
                    ThrowProjectile();
                }
                else if(isGrounded)
                {
                    JumpTowardsPlayer();
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

    private void MeleeAttack()
    {
        if(numAttacksLeft > 0)
        {
            // Attack player
            numAttacksLeft -= 1;
        }
        else if(attackCooldown > 0)
        {
            // Pausing before throwing phase begins
            attackCooldown -= 1;
        }
        else
        {
            // Turn off melee phase and begin throwing phase
            attackCooldown = initAttackCooldown;
            meleePhase = false;
            throwingPhase = true; 
        }
    }

    private void ThrowProjectile()
    {
        if(throwCooldown <=  0)
        {
            throwCooldown = initThrowCooldown;

            if (!GameConfig.Multiplayer)
            {
                Instantiate(projectile, transform.position, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Hazards", "Tomato"), transform.position, Quaternion.identity);
            }

            numThrowsLeft -= 1;

            if (numThrowsLeft == 0)
            {
                numThrowsLeft = initNumThrows;
                throwingPhase = false;
            }
        }
        else
        {
            throwCooldown -= 1;
        }
    }

    private void Retreat()
    {
        if(cowardlyTime > 0)
        {
            // Only retreat from player if they are within retreat range
            if(Math.Abs(xDist) < retreatRange)
            {
                if (xDist < 0)
                {
                    // Walk to the right
                    Vector2 target = new Vector2(rb.position.x - retreatRange, rb.position.y);
                    transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
                }
                else
                {
                    // Walk to the left
                    Vector2 target = new Vector2(rb.position.x + retreatRange, rb.position.y);
                    transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
                }
            }
            cowardlyTime -= 1;
        }
        else
        {
            // Coward phase is over
            cowardlyTime = initCowardlyTime;
            cowardPhase = false;
            throwingPhase = true;
        }
        
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

    private void JumpTowardsPlayer()
    {
        if(jumpsLeft > 0)
        {
            rb.AddForce(new Vector2(-xDist * 5, jumpHeight), ForceMode2D.Impulse);
            jumpsLeft -= 1;
        } 
        else
        {
            // Coward phase begins
            cowardPhase = true;
            jumpsLeft = initJumpsLeft;
        }
    }
}