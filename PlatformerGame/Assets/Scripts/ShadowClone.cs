using UnityEngine;

public class ShadowClone : MonoBehaviour
{
    private Transform player;
    private float delay;
    private Vector3[] positionBuffer;
    private int bufferIndex;
    private SpriteRenderer playerRenderer;
    private SpriteRenderer cloneRenderer;
    
    public void Initialize(Transform playerTransform, float cloneDelay)
    {
        player = playerTransform;
        delay = cloneDelay;
        playerRenderer = player.GetComponent<SpriteRenderer>();
        cloneRenderer = GetComponent<SpriteRenderer>();
        
        // Create buffer to store player positions
        int bufferSize = Mathf.CeilToInt(delay / Time.fixedDeltaTime);
        positionBuffer = new Vector3[bufferSize];
    }
    
    private void FixedUpdate()
    {
        // Record player's current position
        positionBuffer[bufferIndex] = player.position;
        bufferIndex = (bufferIndex + 1) % positionBuffer.Length;
        
        // Move clone to delayed position
        transform.position = positionBuffer[bufferIndex];
        
        // Mirror player's sprite flip state
        cloneRenderer.flipX = playerRenderer.flipX;
    }
    
    private void OnDestroy()
    {
        // Visual effect when clone disappears
        Instantiate(Resources.Load<GameObject>("Effects/ClonePoof"), transform.position, Quaternion.identity);
    }
}
