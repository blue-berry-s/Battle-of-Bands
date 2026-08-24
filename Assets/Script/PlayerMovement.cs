using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//read inputs
public class PlayerMovement : MonoBehaviour
{

    public PlayerController controller;
    public float runSpeed = 20f;
    public Animator animator;


    float horizontalMove = 0f;
    bool jump = false;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("moveSpeed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump")) {
            jump = true;
            animator.SetBool("isJumping", true);
        }

    }

    // Gets called a fixed amount of times per second
    private void FixedUpdate()
    {

        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);

        jump = false;
    }

    public void onLanding() {
        animator.SetBool("isJumping", false);
    }

}
