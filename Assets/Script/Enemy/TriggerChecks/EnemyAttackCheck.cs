using UnityEngine;
using System.Collections.Generic;

public class EnemyAttackCheck : MonoBehaviour
{

    private GameObject player;
    private Enemy enemy;

    private List<Collider2D> objectsInTrigger = new List<Collider2D>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        enemy = GetComponentInParent<Transform>().GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (objectsInTrigger.Count > 0) {
            enemy.setAttackingDistanceBool(true);
        }
        else {
            enemy.setAttackingDistanceBool(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            objectsInTrigger.Add(collision);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            objectsInTrigger.Remove(collision);
            objectsInTrigger.RemoveAll(item => item == null);
        }
    }
}
