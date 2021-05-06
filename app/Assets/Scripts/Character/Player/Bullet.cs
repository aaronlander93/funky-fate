/*
Code By: Andrew Sha

This code damages the enemy when getting hit by this bullet
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet: MonoBehaviour
{
    public int damage = 1;
    public float speed = 10f;
    public float lifeTime = 1f;
    public Rigidbody2D rb;
    public GameObject Effect;
    // Start is called before the first frame update
    private GameSetupController gsc;
    void Start()
    {
        rb.velocity = transform.right * speed;
        Invoke("Particles",lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage, true);
        }
        Instantiate(Effect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void Particles(){
        Instantiate(Effect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}