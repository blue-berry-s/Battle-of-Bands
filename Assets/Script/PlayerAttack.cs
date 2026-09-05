using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private bool canAttack = true;
    private bool isblocking = false;
    public Animator animator;
    public PlayerController controller;
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private InputActionReference kickAction;
    [SerializeField] private InputActionReference blockAction;
    public GameObject[] attackHitBox;
    private Metronome metronome;
    private beatManager beatManager;
    Keyboard keyboard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        metronome = GameObject.FindGameObjectWithTag("Metronome").GetComponent<Metronome>();
        beatManager = FindAnyObjectByType<beatManager>();
    }

    private void OnEnable()
    {
        if (attackAction != null) attackAction.action.Enable();
        if (kickAction != null) kickAction.action.Enable();
        if (blockAction != null) blockAction.action.Enable();
    }

    private void OnDisable()
    {
        if (attackAction != null) attackAction.action.Disable();
        if (kickAction != null) kickAction.action.Disable();
        if (blockAction != null) blockAction.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {

        
        if (attackAction.action.WasPressedThisFrame())
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
        else if (kickAction.action.WasPressedThisFrame())
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
        else if (blockAction.action.WasPressedThisFrame() && !animator.GetBool("isHurt") && !animator.GetBool("isAttacking") && !animator.GetBool("isKicking")) {
            if (!isblocking) {
                isblocking = true;
                canAttack = false;
                animator.SetBool("isAttacking", false);
                animator.SetBool("isKicking", false);
                animator.SetBool("isBlocking", true);

                //Debug.Log("isAttacking: " + animator.GetBool("isAttackign"));
            }
        }

        if (blockAction.action.WasReleasedThisFrame() && isblocking) {
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
