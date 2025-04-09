using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Vector2 direction;
    private float speed = 8f;
    private float lifetime = 1.5f;
    private float damage = 1f;
    private LayerMask enemyLayer;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public void Initialize(Vector2 fireDirection)
    {
        direction = fireDirection;
        enemyLayer = LayerMask.GetMask("Enemy");

        // Set up physics
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0.5f;
        rb.velocity = direction * speed;

        // Add collider
        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
        collider.radius = 0.3f;
        collider.isTrigger = true;

        // Set up visual
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Fireball");
        spriteRenderer.sortingOrder = 5;

        // Destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            // Damage enemy
            other.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            // Bounce off ground
            rb.velocity = new Vector2(rb.velocity.x, speed * 0.7f);
        }
    }
}
