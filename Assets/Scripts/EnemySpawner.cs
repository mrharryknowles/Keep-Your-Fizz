using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab; //reference to the enemy prefab
    public float initialSpawnInterval = 3f; //time between the spawns
    public float spawnRadius = 10f; //distance from the centre to spawn the enemies
    public Transform playerTransform; //reference to the player
    public float intervalScale = 10f; //scaling factor for the spawn-speedup

    private float elapsedTime = 0; //tracks the total elapsed time/game duration

    private float counter = 0f; //timer to track the spawn intervals

    void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime; //update elapsed time, increment every frame
        counter += Time.fixedDeltaTime; //increment the timer

        float spawnInterval = initialSpawnInterval / Mathf.Max(Mathf.Sqrt(elapsedTime / intervalScale), 1);
        //spawn calculated based on the elapsed time

        if (counter > spawnInterval)
        {
            counter = 0f;//reset the timer
            SpawnEnemy(); //spawn an enemy
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
