using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private Vector2 direction;
    private float speed = 15f;
    private float lifetime = 2f;
    private float damage = 1f;
    private LayerMask enemyLayer;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float rotationSpeed = 720f;

    public void Initialize(Vector2 throwDirection, float cooldown)
    {
        direction = throwDirection;
        enemyLayer = LayerMask.GetMask("Enemy");
        
        // Set up visual
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Shuriken");
        spriteRenderer.sortingOrder = 10;
        
        // Set up physics
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = direction * speed;
        
        // Add collider
        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
        collider.radius = 0.3f;
        collider.isTrigger = true;
        
        // Destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Rotate shuriken
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            // Damage enemy
            other.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            // Stick to walls
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            Destroy(this); // Stop rotation and movement
        }
    }
}
