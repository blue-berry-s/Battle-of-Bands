using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool canAttack = true;
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

        if (Input.GetButtonDown("Attack") && canAttack)
        {
            canAttack = false;
            animator.SetBool("isJumping", false);
            animator.SetBool("isAttacking", true);
        }
        else if (Input.GetButtonDown("Kick") && canAttack) {
            canAttack = false;
            animator.SetBool("isJumping", false);
            animator.SetBool("isKicking", true);
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
}
