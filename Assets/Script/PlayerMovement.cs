using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

//read inputs
public class PlayerMovement : MonoBehaviour
{

    public PlayerController controller;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    public float runSpeed = 20f;
    public Animator animator;
    public bool isFrozen = false;


    private float horizontalMove;
    bool jump = false;

    private void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (jumpAction != null) jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (jumpAction != null) jumpAction.action.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetBool("isAttacking") && !animator.GetBool("isBlocking") && !animator.GetBool("isKicking") && !animator.GetBool("isHurt"))
        {
            if (isFrozen)
            {
                unfreezePlayer();
            }

            // 4. NEW MOVEMENT SYSTEM
            // Reads Vector2 directly from your Action Asset configuration (e.g., WASD or D-Pad)
            horizontalMove = moveAction.action.ReadValue<Vector2>().x * runSpeed;

            // Note: If you only need the horizontal X axis, use: Mathf.Abs(horizontalMove.x)
            animator.SetFloat("moveSpeed", Mathf.Abs(horizontalMove));

            // 5. NEW JUMP SYSTEM
            // WasPressedThisFrame() perfectly replaces Input.GetButtonDown()
            if (jumpAction.action.WasPressedThisFrame())
            {
                jump = true;
                animator.SetBool("isJumping", true);
            }
        }
        else if (animator.GetBool("isHurt"))
        {
            if (jump)
            {
                transform.GetComponent<Rigidbody2D>().linearVelocityY = -10;
            }
        }
        else
        {
            if (!isFrozen)
            {
                freezePlayer();
            }
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

    private void freezePlayer() {
        isFrozen = true;
        controller.forwardBlocked(gameObject.GetComponent<Transform>().position.x);
        controller.backwardBlocked(gameObject.GetComponent<Transform>().position.x);
    }

    private void unfreezePlayer() {
        controller.forwardOpen();
        controller.backwardOpen();
        isFrozen = false;
    }


}
