using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool canAttack = true;
    private bool isblocking = false;
    public Animator animator;
    public PlayerController controller;
    public GameObject[] attackHitBox;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetButtonDown("Attack") && canAttack && !animator.GetBool("isHurt"))
        {
            canAttack = false;
            animator.SetBool("isJumping", false);
            animator.SetBool("isAttacking", true);
        }
        else if (Input.GetButtonDown("Kick") && canAttack && !animator.GetBool("isHurt"))
        {
            canAttack = false;
            animator.SetBool("isJumping", false);
            animator.SetBool("isKicking", true);
        }
        else if (Input.GetButtonDown("Block") && !animator.GetBool("isHurt")) {
            if (!isblocking) {
                isblocking = true;
                canAttack = false;
                animator.SetBool("isBlocking", true);
            }
        }

        if (Input.GetButtonUp("Block")) {
            isblocking = false;
            canAttack = true;
            animator.SetBool("isBlocking", false);

        }
    }

    public void isAttacking() {
        attackHitBox[0].SetActive(true);
    }

    public void stopAttacking()
    {
        attackHitBox[0].SetActive(false);
        animator.SetBool("isAttacking", false);
        if (!controller.isGrounded()) {
            animator.SetBool("isJumping", true);
        }

        canAttack = true;
    }

    public void isKicking() {
        attackHitBox[1].SetActive(true);
    }

    public void stopKicking() {
        attackHitBox[1].SetActive(false);
        animator.SetBool("isKicking", false);
        if (!controller.isGrounded())
        {
            animator.SetBool("isJumping", true);
        }

        canAttack = true;
    }

    public void blocking() { 
        
    }
}
