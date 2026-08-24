using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
 
    public PlayerController controller;
    public bool isFront = true;

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
        if (collision.gameObject.layer == 7) {
            if (isFront)
            {

                controller.forwardBlocked(playerTransform.position.x);
            }
            else
            {

                controller.backwardBlocked(playerTransform.position.x);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
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
    }
}
