using UnityEngine;

public class PoloEnemyController : MonoBehaviour
{
    public float health = 50f; //assigned health of the polo
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

    //method to take damage from the player/can
    public void TakeDamage(float damage)
    {
        health -= damage;

        //if their health is less than or equal to 0, destroy the enemy
        if (health <= 0)
        {
            Destroy(gameObject); // destroy the enemy
        }
    }

    //detect collision with the cola can
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //calls TakeDamage when the enemy collides with the player
            TakeDamage(damageAmount);
        }
    }
}
