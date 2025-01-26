using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 30f;

    public Sprite fullHealthSprite;
    public Sprite midHealthSprite;
    public Sprite lowHealthSprite;

    public SpriteRenderer spriteRenderer;

    //method to take damage from the player/can
    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        UpdateSprite();

        //if their health is less than or equal to 0, destroy the enemy
        if (health <= 0)
        {
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