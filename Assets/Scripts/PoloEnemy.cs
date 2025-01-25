using UnityEngine;

public class PoloEnemyController : Enemy
{
    public float acceleration = 3f;
    public float maxVelocity = 1f;

    public float damageAmount = 10f; //damage dealt to the player when colliding
    private Transform playerTransform; //reference to the player

    private Rigidbody2D rb;

    private float disableUntil;
    public float disableFor = 1f;

    private void Start()
    {
        //finds the player via their tag
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //moves towards the player
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (playerTransform != null && Time.time > disableUntil)
        {
            //calculate direction towards the player
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            //move the enemy towards the player
            rb.AddForce(direction * acceleration);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
        }
    }

    //detect collision with the cola can
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<ColaCanController>().DamagePlayer(damageAmount, transform);
            disableUntil = Time.time + disableFor/2f;
        }
    }

    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);
        disableUntil = Time.time + disableFor;
    }
}
