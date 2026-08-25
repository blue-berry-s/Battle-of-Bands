using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{
    public float maxHealth { get; set; } = 10;
    public float currentHealth { get; set; }

    public bool canBeDamaged { get; set; } = true;

    public Animator enemyAnimator;
    public Rigidbody2D enemyRigidBody;

    public void Damage(float damageAmount)
    {

        if (canBeDamaged) {
            canBeDamaged = false;
            if (currentHealth - damageAmount > 0)
            {
                enemyRigidBody.AddForce(new Vector2(50, 100));
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void recover() {
        enemyAnimator.SetBool("isHurt", false);
        canBeDamaged = true;
        
    }
}
