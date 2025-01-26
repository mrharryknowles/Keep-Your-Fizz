using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 30f;

    public Sprite fullHealthSprite;
    public Sprite midHealthSprite;
    public Sprite lowHealthSprite;

    public SpriteRenderer spriteRenderer;
    public float rotationSpeed = 10f;

    private float currentRotationSpeed;

    public GameObject deathParticles;

    private void RandomiseRotationSpeed()
    {
        currentRotationSpeed = Random.Range(-rotationSpeed, rotationSpeed);

    }

    private void Awake()
    {
        RandomiseRotationSpeed();
    }

    private void Update()
    {
        SpinEnemy();
    }


    private void SpinEnemy()
    {
        transform.Rotate(Vector3.forward * currentRotationSpeed * Time.deltaTime);
    }

    //method to take damage from the player/can
    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        RandomiseRotationSpeed();

        UpdateSprite();

        //if their health is less than or equal to 0, destroy the enemy
        if (health <= 0)
        {
            Instantiate(deathParticles, transform.position, transform.rotation);
            Destroy(gameObject); // destroy the enemy
        }
    }

    private void UpdateSprite()
    {
        if (health > 20)
        {
            spriteRenderer.sprite = fullHealthSprite;
        }
        else if (health > 10)
        {
            spriteRenderer.sprite = midHealthSprite;
        }
        else
        {
            spriteRenderer.sprite = lowHealthSprite;
        }
        
    }
}