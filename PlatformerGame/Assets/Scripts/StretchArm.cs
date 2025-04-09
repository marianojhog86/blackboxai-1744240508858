using UnityEngine;

public class StretchArm : MonoBehaviour
{
    private Transform player;
    private float maxLength;
    private Vector2 direction;
    private LineRenderer lineRenderer;
    private float currentLength;
    private bool isRetracting;
    private float speed = 15f;
    private LayerMask enemyLayer;

    public void Initialize(Transform playerTransform, float punchRange, Vector2 punchDirection)
    {
        player = playerTransform;
        maxLength = punchRange;
        direction = punchDirection;
        enemyLayer = LayerMask.GetMask("Enemy");

        // Set up visual representation
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.3f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = new Color(1, 0.5f, 0); // Orange
        lineRenderer.positionCount = 2;

        // Start extending
        currentLength = 0;
        isRetracting = false;
    }

    private void Update()
    {
        if (!isRetracting)
        {
            // Extend arm
            currentLength += speed * Time.deltaTime;
            if (currentLength >= maxLength)
            {
                isRetracting = true;
            }
        }
        else
        {
            // Retract arm
            currentLength -= speed * Time.deltaTime;
            if (currentLength <= 0)
            {
                Destroy(gameObject);
                return;
            }
        }

        UpdateArmPosition();
        CheckForHits();
    }

    private void UpdateArmPosition()
    {
        Vector2 endPoint = (Vector2)player.position + direction * currentLength;
        lineRenderer.SetPosition(0, player.position);
        lineRenderer.SetPosition(1, endPoint);
    }

    private void CheckForHits()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            player.position,
            direction,
            currentLength,
            enemyLayer
        );

        if (hit.collider != null)
        {
            // Damage enemy
            hit.collider.GetComponent<Enemy>().TakeDamage(1);
            isRetracting = true;
        }
    }
}
