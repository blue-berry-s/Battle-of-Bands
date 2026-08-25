using UnityEngine;
using System.Collections.Generic;

public class PlayerCollision : MonoBehaviour
{
 
    public PlayerController controller;
    public bool isFront = true;

    private List<Collider2D> objectsInTrigger = new List<Collider2D>();
    private float prevCount = 0;


    private Transform playerTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            if (objectsInTrigger.Count == 0) {
                if (isFront)
                {
                    controller.forwardBlocked(playerTransform.position.x);
                }
                else
                {
                    controller.backwardBlocked(playerTransform.position.x);
                }
            }
            objectsInTrigger.Add(collision);
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            objectsInTrigger.Remove(collision);
        }

        // Clean up any destroyed or null objects to prevent ghost entries
        objectsInTrigger.RemoveAll(item => item == null);

        // Check if the trigger is now completely empty
        if (objectsInTrigger.Count == 0 && prevCount > 0)
        {
            if (isFront)
            {
                controller.forwardOpen();
            }
            else
            {
                controller.backwardOpen();
            }
        }
        prevCount = objectsInTrigger.Count; 
    }
}
