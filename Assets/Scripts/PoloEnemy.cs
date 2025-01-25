using UnityEngine;

public class PoloEnemyController : Enemy
{
    public float moveSpeed = 3f; //assigned speed of the polo
    public float damageAmount = 10f; //damage dealt to the player when colliding
    private Transform playerTransform; //reference to the player

    private void Start()
    {
        //finds the player via their tag
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        //moves towards the player
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (playerTransform != null)
        {
            //calculate direction towards the player
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            //move the enemy towards the player
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    //detect collision with the cola can
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //damage player
        }
    }
}
