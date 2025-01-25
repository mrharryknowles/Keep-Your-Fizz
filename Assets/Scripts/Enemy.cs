using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 50f;

    //method to take damage from the player/can
    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        //if their health is less than or equal to 0, destroy the enemy
        if (health <= 0)
        {
            Destroy(gameObject); // destroy the enemy
        }
    }
}