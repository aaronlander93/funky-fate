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
    [SerializeField] private bool dead = false;
    [SerializeField] private int health = 5;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

    }

    private void BounceBack(bool damageFromRight)
    {
        if (damageFromRight)
        {
            rb.AddForce(new Vector2(-50f, 0), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(50f, 0), ForceMode2D.Impulse);
        }
        
    }
    

    private void Die()
    {
        dead = true;

        gameObject.transform.Rotate(0, 0, 90);
        Destroy(GetComponent<EnemyAI>());
    }

    public bool isDead()
    {
        return dead;
    }

    public void TakeDamage (int damage, bool damageFromRight)
    {
        health -= damage;
        
        if(health <= 0)
        {
            Die();
        }
        else
        {
            BounceBack(damageFromRight);
        }
    }

    
}
