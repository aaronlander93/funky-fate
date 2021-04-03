/*
Code By: Andrew Sha
This code is for the animation of the character.
Drag this code to the entity that needs animation.
Add variables in the animator:

Speed
isJumping
isCrouching
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;
    public float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    // Update is called once per frame

    public float knockback;
    public float knockbackLength;
    public float knockbackCount;
    public bool knockFromRight;

    [SerializeField] private float jumpBufferTime = .1f;

    private float jumpBufferCounter;
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
            jump = true;
            animator.SetBool("isJumping", true);
        }
        else {
            jumpBufferCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        } 
    }
    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("isCrouching", isCrouching);
    }
    private void FixedUpdate()
    {
        if (knockbackCount <= 0)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        }
         else
        {
            if (knockFromRight)
                controller.m_Rigidbody2D.velocity = new Vector2(-knockback, knockback);
            if (!knockFromRight)
                controller.m_Rigidbody2D.velocity = new Vector2(knockback, knockback);
            knockbackCount -= Time.deltaTime;
        }
        jump = false;
        
    }
}
