using UnityEngine;

public interface IHealth 
{
    bool canBeDamaged { get; set; }
    float maxHealth { get; set; }
    float currentHealth { get; set; }

    public void Damage(float damageAmount);

    void Die();
}
