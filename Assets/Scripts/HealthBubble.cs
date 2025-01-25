using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBubble : MonoBehaviour
{
    [SerializeField] private float fizzinessBoost = 10f; //amount of fizziness to increase on collision
    [SerializeField] private float lifetime = 5f; //how long the health bubbles exist before getting destroyed

    private void Start()
    {
        //destroy the health bubble after the specified amount of time
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //checks if the player/Can collides with the HealthBubble
        if (other.CompareTag("Player"))
        {
            //get the ColaCanController script attached to the player/Can
            ColaCanController colaCan = other.GetComponent<ColaCanController>();

            if (colaCan != null)
            {
                //increases the player's fizziness/health
                colaCan.IncreaseFizziness(fizzinessBoost);
            }

            //destroys the bubble on collision
            Destroy(gameObject);
        }
    }
}
