using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoloEnemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50f; //max enemy health
    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = maxHealth; //set the initial health
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // checks whether the player is slamming
            ColaCanController player = collision.gameObject.GetComponent<ColaCanController>();
            if (player != null && player.IsSlamming())
            {
                TakeDamage(25f); //if slamming, take damage
            }
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Polo Enemy destroyed!");
        Destroy(gameObject); //destroy enemy
    }
} 