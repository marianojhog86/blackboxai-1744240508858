using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public int currentLevel = 1;
    public int lives = 3;
    public int coins = 0;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void LoadLevel(int levelIndex)
    {
        currentLevel = levelIndex;
        SceneManager.LoadScene("Level_" + levelIndex);
    }
    
    public void AddCoin()
    {
        coins++;
        if (coins >= 100)
        {
            coins -= 100;
            AddLife();
        }
    }
    
    public void AddLife()
    {
        lives++;
    }
    
    public void PlayerDied()
    {
        lives--;
        if (lives > 0)
        {
            LoadLevel(currentLevel);
        }
        else
        {
            GameOver();
        }
    }
    
    private void GameOver()
    {
        // Reset game state
        lives = 3;
        coins = 0;
        currentLevel = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
