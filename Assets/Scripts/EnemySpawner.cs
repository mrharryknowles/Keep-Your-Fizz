using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab; //reference to the enemy prefab
    public float spawnInterval = 3f; //time between the spawns
    public float spawnRadius = 10f; //distance from the centre to spawn the enemies
    public Transform playerTransform; //reference to the player

    private float counter = 0f; //timer to track the spawn intervals

    void FixedUpdate()
    {
        counter += Time.fixedDeltaTime; //increment the timer

        if (counter > spawnInterval)
        {
            counter = 0f;//reset the timer
            SpawnEnemy();
        }

    }

    private void SpawnEnemy()
    {
        //generate a random angle for the spawn position
        float angle = Random.Range(0f, 360f);
        Vector2 spawnPosition = new Vector2(
            playerTransform.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * spawnRadius,
            playerTransform.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * spawnRadius
        );

        //instantiate the enemy at the calculated position
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
