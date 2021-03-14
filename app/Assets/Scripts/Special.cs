/*
Code By: Andrew Sha
Code for special attack.
Instantiates a "bullet" to fly out

In Game settings, you can change Fire1 key

IN ANIMATOR:

make variable isSpecial
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special : MonoBehaviour
{
    public Transform ShootPoint;
    public GameObject bulletPrefab;
    public Animator animator;
    public float shotDelay;
    private float shotDelayCounter;
    public string button = "Fire1";
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(button))
        {
            Shoot();
            shotDelayCounter = shotDelay;
            animator.SetBool("isSpecial", true);
        }
        else if(Input.GetButtonUp(button))
        {
            animator.SetBool("isSpecial", false);
        }
        if(Input.GetKey(button))
        {
            shotDelayCounter -= Time.deltaTime;
            if(shotDelayCounter <= 0)
            {
                shotDelayCounter = shotDelay;
                Shoot();
                
            }
        }
    }
    void Shoot()
    {
        Instantiate(bulletPrefab, ShootPoint.position, ShootPoint.rotation);
    }
}
