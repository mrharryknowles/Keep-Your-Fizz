using System.Collections;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject healthBubblePrefab; //reference to the ColaBubble prefab
    [SerializeField] private float spawnInterval = 2f; // how often health bubbles spawn (in seconds)
    [SerializeField] private float spawnAreaWidth = 10f; // width of the spawn area (left to right)
    [SerializeField] private float spawnHeight = 6f; // height from which the health bubbles will fall (top of the screen)

    [SerializeField] private float doubleBubbleSpawnDuration = 60f;

    private void Start()
    {
        //start the spawning process
        StartCoroutine(SpawnHealthBubbles());
    }

    private IEnumerator SpawnHealthBubbles()
    {
        while (true)
        {
            //wait for the next spawn interval
            yield return new WaitForSeconds(spawnInterval / Mathf.Max(2 - Time.timeSinceLevelLoad/doubleBubbleSpawnDuration, 1));

            //generate a random position along the top of the screen (within spawnAreaWidth)
            Vector2 spawnPosition = new Vector2(
                Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2),  //random X position within the spawn area
                spawnHeight //fixed Y position at the top of the screen
            );

            //instantiate a new health bubble at the random position
            Instantiate(healthBubblePrefab, spawnPosition, Quaternion.identity);
        } 
    }
}