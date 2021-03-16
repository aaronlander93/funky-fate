/*
Code By: Andrew Sha
Enemy can get damaged code for testing purposes.
Gives points to players.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int EnemyHealth = 100;
    public float speed = 40f;
    public float startTimeBtwShots;
    private float TimeBtwShots;

    public GameObject deathEffect;

    public GameObject EnemyShuriken;
    private Transform player;

    public int pointsOnDeath;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }
    private void Update()
    {
        
        //Shooting Enemy
        if(startTimeBtwShots <= 0)
        {
            Instantiate(EnemyShuriken, transform.position, Quaternion.identity);
            TimeBtwShots = startTimeBtwShots;
        }
        else
        {
            TimeBtwShots -= Time.deltaTime;
        }
    }
    public void TakeDamage (int damage)
    {
        EnemyHealth -= damage;
        
        if(EnemyHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
