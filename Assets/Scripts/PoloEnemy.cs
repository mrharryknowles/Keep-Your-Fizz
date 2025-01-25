using UnityEngine;

public class PoloEnemyController : MonoBehaviour
{
    public float health = 50f; // Assigned health of the polo
    public float moveSpeed = 3f; // Assigned speed of the polo
    public float damageAmount = 10f; // Damage dealt to the player when colliding
    private Transform playerTransform; // Reference to the player

    private void Start()
    {
        // Finds the player via their tag
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Moves towards the player
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (playerTransform != null)
        {
            // Calculate direction towards the player
            Vector2 direction = (playerTransform.position - transform.position).normalized;

            // Move the enemy towards the player
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }

    // Method to handle taking damage from the cola can
    public void TakeDamage(float damage)
    {
        health -= damage;

        // If health is less than or equal to 0, destroy the enemy
        if (health <= 0)
        {
            Destroy(gameObject); // Destroys the enemy
        }
    }

    // Detect collision with the cola can
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Calls TakeDamage when the enemy collides with the player
            TakeDamage(damageAmount);
        }
    }
}
