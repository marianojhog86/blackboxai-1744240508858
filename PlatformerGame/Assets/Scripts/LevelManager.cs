using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum WorldType { Mario, Naruto, OnePiece }
    public WorldType currentWorld;
    
    [Header("World Settings")]
    public float marioGravity = 3f;
    public float narutoGravity = 2.5f;
    public float onePieceGravity = 2f;
    
    [Header("Spawn Points")]
    public Transform playerSpawnPoint;
    public Transform[] enemySpawnPoints;
    
    private void Start()
    {
        SetWorldPhysics();
        SpawnPlayer();
        SpawnEnemies();
    }
    
    private void SetWorldPhysics()
    {
        switch(currentWorld)
        {
            case WorldType.Mario:
                Physics2D.gravity = new Vector2(0, -marioGravity * 9.81f);
                break;
            case WorldType.Naruto:
                Physics2D.gravity = new Vector2(0, -narutoGravity * 9.81f);
                break;
            case WorldType.OnePiece:
                Physics2D.gravity = new Vector2(0, -onePieceGravity * 9.81f);
                break;
        }
    }
    
    private void SpawnPlayer()
    {
        // Player spawning logic will be handled by GameManager
    }
    
    private void SpawnEnemies()
    {
        // Enemy spawning logic will be implemented later
    }
    
    public void CompleteLevel()
    {
        GameManager.Instance.LoadLevel(GameManager.Instance.currentLevel + 1);
    }
}
