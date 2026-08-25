using UnityEngine;

public class playerHealth : MonoBehaviour, IHealth
{

    public Animator animator;
    public bool canBeDamaged { get; set; } = true;
    public float maxHealth { get; set; } = 10;
    public float currentHealth { get; set ; }

    public void Damage(float damageAmount)
    {
        if (canBeDamaged && !animator.GetBool("isBlocking"))
        {
            canBeDamaged = false;
            if (currentHealth - damageAmount > 0)
            { 
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(50, 100));
                currentHealth -= damageAmount;
                animator.SetBool("isHurt", true);
            }
            else
            {
                currentHealth = 0;
                Die();
            }
        }
    }

    public void Die()
    {
        Debug.Log("PLAYER DIED");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void recover()
    {
        animator.SetBool("isHurt", false);
        canBeDamaged = true;

    }
}
