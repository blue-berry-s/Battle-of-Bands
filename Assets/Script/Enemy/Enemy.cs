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

    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdle idleState { get; set; }
    public EnemyMove moveState { get; set; }
    public EnemyAttack attackState { get; set; }

    public EnemyBlock blockState { get; set; }

    public bool isWithinAttackingDistance { get; set; }
    public bool isWithinKickingDistance { get; set; }

    public GameObject[] attackHitBox;

    public Enemydata data;

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();
        idleState = new EnemyIdle(this, StateMachine);
        moveState = new EnemyMove(this, StateMachine);
        attackState = new EnemyAttack(this, StateMachine);
        blockState = new EnemyBlock(this, StateMachine);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        currentHealth = data.maxHealth;
        StateMachine.Initalize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
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
        StateMachine.currentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        
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

    public void moveEnemy(Vector2 direction) {
        if (!enemyAnimator.GetBool("isHurt")) {
            enemyAnimator.SetBool("isMoving", true);
            enemyRigidBody.linearVelocityX = direction.x * data.movementSpeed;
        }
    }

    public void attackPlayer() {
        if (!enemyAnimator.GetBool("isAttacking")) {
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
        enemyAnimator.SetBool("isAttacking", true);
    }

    IEnumerator delayBlock(float time) {
        yield return new WaitForSeconds(time);
        StateMachine.ChangeState(blockState);
    }

}
