using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IHealth, ITriggerCheckable
{

    private Transform player;
    public float maxHealth { get; set; }
    public float currentHealth { get; set; }

    public bool canBeDamaged { get; set; } = true;

    public Animator enemyAnimator;
    public Rigidbody2D enemyRigidBody;
    public Metronome metronome;

    // State Machine
    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdle idleState { get; set; }
    public EnemyMove moveState { get; set; }
    public EnemyAttack attackState { get; set; }

    public EnemyBlock blockState { get; set; }

    public EnemyRetreat retreatState { get; set; }

    public EnemyJump jumpState { get; set; }

    public bool isWithinAttackingDistance { get; set; }
    public bool isWithinKickingDistance { get; set; }

    public GameObject[] attackHitBox;

    public Enemydata data;

    public HealthBar HealthUI;

    public float outOfBounds { get; private set; } = 8.5f;

    private bool m_Grounded;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private LayerMask m_WhatIsGround;
    const float k_GroundedRadius = .17f; // Radius of the overlap circle to determine if grounded

    public bool doneBlocking = false;

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();
        idleState = new EnemyIdle(this, StateMachine);
        moveState = new EnemyMove(this, StateMachine);
        attackState = new EnemyAttack(this, StateMachine);
        blockState = new EnemyBlock(this, StateMachine);
        retreatState = new EnemyRetreat(this, StateMachine);
        jumpState = new EnemyJump(this, StateMachine);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        currentHealth = data.maxHealth;
        StateMachine.Initalize(idleState);
        HealthUI.setMaxHealth(Mathf.RoundToInt(currentHealth));
        metronome = GameObject.FindGameObjectWithTag("Metronome").GetComponent<Metronome>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log("Current State: " + StateMachine.currentEnemyState);
        if (Input.GetButtonDown("Attack") || Input.GetButtonDown("Kick"))
        {
            Vector2 distance = player.position - transform.position;
            //Debug.Log("X: " + distance.x + " Y: " + distance.y);
            if (Mathf.Abs(distance.x) <= 7 && Mathf.Abs(distance.y) <= 1)
            {
                float rand = Random.Range(0f, 1f);
                if (rand < data.blockingChance) {
                    float randFloat = Random.Range(0, data.maxReactionDelay);
                    StartCoroutine(delayBlock(randFloat));
                }
            }

        }
        else if (isWithinAttackingDistance && StateMachine.currentEnemyState != attackState)
        {
            if (metronome.attackPeriod)
            {
                StateMachine.ChangeState(attackState);
            }
        }
        StateMachine.currentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    enemyAnimator.SetBool("isJumping", false);
            }
        }
        StateMachine.currentEnemyState.PhysicsUpdate();
    }

    private void AnimationTriggerEvent(AnimationTriggerType triggerType) {
        StateMachine.currentEnemyState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType {

    }

    public void Damage(float damageAmount)
    {

        if (canBeDamaged && !enemyAnimator.GetBool("isBlocking")) {
            canBeDamaged = false;
            if (currentHealth - damageAmount > 0)
            {
                enemyRigidBody.AddForce(new Vector2(400, 200));
                currentHealth -= damageAmount;
                enemyAnimator.SetBool("isHurt", true);
                
            }
            else
            {
                currentHealth = 0;
                Die();
            }

            HealthUI.setHealth(Mathf.RoundToInt(currentHealth));
        }
        //Debug.Log(currentHealth);
    }

    public void Die()
    {
        Debug.Log("I DIED!");
    }


    public void recover() {
        enemyAnimator.SetBool("isHurt", false);
        canBeDamaged = true;

    }

    public void moveEnemy(Vector2 direction)
    {
        if (!enemyAnimator.GetBool("isHurt"))
        {
            enemyAnimator.SetBool("isMoving", true);

            // direction.x will now safely be 1 or -1, yielding a rock-solid, constant speed
            enemyRigidBody.linearVelocityX = direction.x * data.movementSpeed;
        }
    }

    public void attackPlayer() {
        if (!enemyAnimator.GetBool("isAttacking")) {
            enemyRigidBody.linearVelocityX = 0;
            float randFloat = Random.Range(data.minReactionDelay, data.maxReactionDelay);
            StartCoroutine(delayAttack(randFloat));
        }
    }

    public void setAttackingDistanceBool(bool canAttack)
    {
        isWithinAttackingDistance = canAttack;
    }

    public void setKickingDistanceBool(bool canKick)
    {
        isWithinKickingDistance = canKick;
    }

    public void isAttacking() {
        attackHitBox[0].SetActive(true);
    }

    public void stopAttacking()
    {
        attackHitBox[0].SetActive(false);
        enemyAnimator.SetBool("isAttacking", false);
    }

    IEnumerator delayAttack(float time)
    {
        yield return new WaitForSeconds(time);
        if (metronome.attackPeriod && !enemyAnimator.GetBool("isAttacking")) {
            //Debug.Log("ATTACKING NOW AND BEAT IS " + metronome.attackPeriod);
            enemyRigidBody.linearVelocityX = 0;
            enemyAnimator.SetBool("isAttacking", true);
        }
        else {
            StateMachine.ChangeState(retreatState);
        }
       
        
    }

    IEnumerator delayBlock(float time) {
        yield return new WaitForSeconds(time);
        StateMachine.ChangeState(blockState);
    }

    public void performBlock() {
        float randFloat = Random.Range(0, data.maxReactionDelay);
        StartCoroutine(delayBlock(randFloat));
    }

    public void jumpForward() {
        enemyRigidBody.AddForce(new Vector2(-3, 0f));

    }

    public void jumpUp() {
        enemyAnimator.SetBool("isJumping", true);
        enemyRigidBody.AddForce(new Vector2(0f, data.jumpForce));
    }

    public void jumpBack() {
        enemyRigidBody.AddForce(new Vector2(3, 0f));
        // Re-trigger idle to pick a safe posture position
    }

    public bool performBlockWait() {
        StartCoroutine(blockWait());
        return true;
    }

    public IEnumerator blockWait() {
        enemyRigidBody.linearVelocityX = 0;
        float time = Random.Range(data.minReactionDelay, data.minReactionDelay);
        yield return new WaitForSeconds(time);
        doneBlocking = true;
    }


}
