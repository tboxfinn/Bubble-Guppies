using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int playerLives = 3;
    public float gameSpeed = 1.0f;
    public int score = 0;
    public float minigameDuration = 10.0f;
    public float minigameTimer = 0.0f;
    public bool isMinigameActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartMinigame();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMinigameActive)
        {
            minigameTimer -= Time.deltaTime * gameSpeed;
            if (minigameTimer <= 0)
            {
                FailMinigame();
            }
        }
    }

    public void StartMinigame()
    {
        minigameTimer = minigameDuration;
        isMinigameActive = true;
        // Initialize minigame here
    }

    void CompleteMinigame()
    {
        isMinigameActive = false;
        gameSpeed += 0.1f; // Increase game speed
        StartMinigame(); // Start next minigame
    }

    void FailMinigame()
    {
        isMinigameActive = false;
        playerLives--;
        if (playerLives > 0)
        {
            StartMinigame(); // Start next minigame
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        // Handle game over logic here
        Debug.Log("Game Over");
    }
}
