using UnityEngine;
[CreateAssetMenu(menuName = "Enemy Data")]
public class Enemydata : ScriptableObject
{
    [field: SerializeField] public float minReactionDelay { get; private set; }
    [field: SerializeField] public float maxReactionDelay { get; private set; }
    [field: SerializeField] public float blockingChance { get; private set; }
    [field: SerializeField] public float movementSpeed { get; private set; }
    [field: SerializeField] public float maxHealth { get; private set; }
    [field: SerializeField] public float jumpForce { get; private set; }


}
