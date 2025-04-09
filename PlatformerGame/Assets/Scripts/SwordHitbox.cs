using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    private Transform player;
    private float damage;
    private Vector2 direction;
    private float duration = 0.2f;
    private float range = 1.5f;
    private LayerMask enemyLayer;
    private SpriteRenderer spriteRenderer;

    public void Initialize(Transform playerTransform, float swordDamage, Vector2 attackDirection)
    {
        player = playerTransform;
        damage = swordDamage;
        direction = attackDirection;
        enemyLayer = LayerMask.GetMask("Enemy");

        // Set up visual
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/SwordSlash");
        spriteRenderer.flipX = direction.x < 0;
        spriteRenderer.sortingOrder = 5;

        // Position the hitbox in front of player
        transform.position = (Vector2)player.position + direction * range/2;
        transform.localScale = new Vector3(range, 0.5f, 1);

        // Destroy after duration
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            // Damage enemy
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
