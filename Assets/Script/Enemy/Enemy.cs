using UnityEngine;

public class Enemy : MonoBehaviour, IHealth, ITriggerCheckable
{
    public float maxHealth { get; set; } = 10;
    public float currentHealth { get; set; }

    public bool canBeDamaged { get; set; } = true;

    public Animator enemyAnimator;
    public Rigidbody2D enemyRigidBody;

    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdle idleState { get; set; }
    public EnemyMove moveState { get; set; }
    public EnemyAttack attackState { get; set; }
    public bool isWithinAttackingDistance { get; set; }
    public bool isWithinKickingDistance { get; set; }

    public GameObject[] attackHitBox;

    //this should be customaizble as a scriptable object
    public float moveSpeed = 1f;
    //attack size - this should also be scriptable object
    //maybe? I need to account for jumping
    public float attackSize = 2f;

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();
        idleState = new EnemyIdle(this, StateMachine);
        moveState = new EnemyMove(this, StateMachine);
        attackState = new EnemyAttack(this, StateMachine);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        StateMachine.Initalize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
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
            enemyRigidBody.linearVelocityX = direction.x * moveSpeed;
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

}
