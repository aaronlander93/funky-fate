/*
Code By: Milo Abril
manages boss health and death animation
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

public class Boss : MonoBehaviour
{
    [SerializeField] private bool dead = false;
    [SerializeField] private int health = 50;
    private bool canTakeDmg = false;
    private Rigidbody2D rb;

    public GameSetupController gsc;
    private Animator _anim;

    private void Start()
    {
        gsc = GameObject.Find("GameSetupController").GetComponent<GameSetupController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }
    
    public void TakeDamage (int damage)
    {
        if (canTakeDmg)
        {
            health -= damage;
        }
        
        if(health <= 0)
        {
            triggerDeath();
        }
    }
    
    private void triggerDeath()
    {
        dead = true;

        //boss will explode

        death();
    }

    private void death()
    {
        gsc.RemoveBoss(rb);

        if (!GameConfig.Multiplayer)
            Destroy(gameObject);
        else if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(gameObject);
    }

    public bool isDead()
    {
        return dead;
    }

    public void setDmgState(bool state)
    {
        canTakeDmg = state;
    }
}
