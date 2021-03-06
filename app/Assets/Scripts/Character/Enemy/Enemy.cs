﻿/*
Code By: Andrew Sha
Enemy can get damaged code for testing purposes.
Gives points to players.
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

public class Enemy : MonoBehaviour
{
    [SerializeField] private bool dead = false;
    [SerializeField] private int health = 5;
    private Rigidbody2D rb;

    public GameSetupController gsc;
    private Animator _anim;

    public GameObject Explosion;

    private void Start()
    {
        gsc = GameObject.Find("GameSetupController").GetComponent<GameSetupController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {

    }

    private void BounceBack(bool damageFromRight)
    {
        if (damageFromRight)
        {
            rb.AddForce(new Vector2(-10f, 0), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(10f, 0), ForceMode2D.Impulse);
        }
        
    }
    
    private void triggerDeath()
    {
        dead = true;

        if (!GameConfig.Multiplayer)
            Instantiate(Explosion, transform.position, Quaternion.identity);
        else
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "FX", "Explosion"), transform.position, Quaternion.identity);

        death();
    }

    private void death()
    {
        gsc.RemoveEnemy(rb);

        if (!GameConfig.Multiplayer)
            Destroy(gameObject);
        else if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(gameObject);
    }

    public bool isDead()
    {
        return dead;
    }

    public void TakeDamage (int damage, bool damageFromRight)
    {
        _anim.SetTrigger("hurt");
        health -= damage;
        
        if(health <= 0)
        {
            triggerDeath();
        }
        else
        {
            BounceBack(damageFromRight);
        }
    }
}
