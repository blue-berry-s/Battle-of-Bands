using UnityEngine;

public class Attack : MonoBehaviour, IAttack
{
    public float damageAmount { get; set; }
    public Collider2D attackHitBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damageAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<Enemy>().Damage(damageAmount);
        }
    }

}
