using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool canAttack = true;
    private bool isblocking = false;
    public Animator animator;
    public PlayerController controller;
    public GameObject[] attackHitBox;
    private Metronome metronome;
    private beatManager beatManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        metronome = GameObject.FindGameObjectWithTag("Metronome").GetComponent<Metronome>();
        beatManager = FindAnyObjectByType<beatManager>();
    }

    // Update is called once per frame
    void Update()
    {

        
        if (Input.GetButtonDown("Attack"))
        {
            if (metronome.attackPeriod) {
                if (canAttack && !animator.GetBool("isHurt")) {
                    canAttack = false;
                    animator.SetBool("isJumping", false);
                    animator.SetBool("isAttacking", true);
                }
            }
            else {
                penalizePlayer();
            }
            
        }
        else if (Input.GetButtonDown("Kick"))
        {
            if (metronome.attackPeriod)
            {
                if (canAttack && !animator.GetBool("isHurt"))
                {
                    canAttack = false;
                    animator.SetBool("isJumping", false);
                    animator.SetBool("isKicking", true);
                }
            }
            else
            {
                penalizePlayer();
            }
        }
        // you can't cancle an attack animation to block
        else if (Input.GetButtonDown("Block") && !animator.GetBool("isHurt") && !animator.GetBool("isAttacking") && !animator.GetBool("isKicking")) {
            if (!isblocking) {
                isblocking = true;
                canAttack = false;
                animator.SetBool("isAtacking", false);
                animator.SetBool("isKicking", false);
                animator.SetBool("isBlocking", true);

                //Debug.Log("isAttacking: " + animator.GetBool("isAttackign"));
            }
        }

        if (Input.GetButtonUp("Block")) {
            isblocking = false;
            canAttack = true;
            stopAttacking();
            stopKicking();
            animator.SetBool("isBlocking", false);

            //Debug.Log("isAttacking: " + animator.GetBool("isAttackign"));

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

    private void penalizePlayer() {
        gameObject.GetComponent<playerHealth>().Damage(metronome.penaltyDamage);
        Intervals interval = beatManager._intervals[0];
        //Debug.Log("_interval: " + interval._interval);
        //Debug.Log("currentInterval: " + interval.currentInterval);
        //Debug.Log("currentInterval: " + interval.lastInterval);

        if (interval._interval%1 >= 0.5f && interval._interval % 1 <= 0.999f)
        {
            Debug.Log("TOO EARLY");
        }
        else if (interval._interval % 1 >= 0.1f && interval._interval % 1 < 0.5)
        {
            Debug.Log("TOO LATE");
        }


    }
}
