using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool canAttack = true;
    public Animator animator;
    public PlayerController controller;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown("Attack") && canAttack) {
            canAttack = false;
            animator.SetBool("isJumping", false);
            animator.SetBool("isAttacking", true);
        }
    }

    public void isAttacking() {
        
    }

    public void stopAttacking()
    {
        animator.SetBool("isAttacking", false);
        if (!controller.isGrounded()) {
            animator.SetBool("isJumping", true);
        }

        canAttack = true;
    }
}
