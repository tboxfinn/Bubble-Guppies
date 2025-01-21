using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Minigames Settings")]
    public GameObject[] minigames; // Lista de prefabs de minijuegos
    public float minigameDuration = 10.0f;

    [Header("Game Settings")]
    public int playerLives = 3;
    public float gameSpeed = 1.0f;

    [Header("UI Settings")]
    public GameObject gameOverScreen; // Pantalla de Game Over (opcional)
    
    private float minigameTimer;
    private bool isMinigameActive = false;
    private int currentMinigameIndex = -1; // Ningún minijuego activo inicialmente
    private int lastMinigameIndex = -1; // Índice del último minijuego

    void Start()
    {
        StartNextMinigame();
    }

    void Update()
    {
        if (isMinigameActive)
        {
            // Actualiza el temporizador del minijuego
            minigameTimer -= Time.deltaTime * gameSpeed;
            if (minigameTimer <= 0)
            {
                FailMinigame();
            }
        }
    }

    void StartNextMinigame()
    {
        if (currentMinigameIndex >= 0)
        {
            // Desactiva el minijuego anterior
            minigames[currentMinigameIndex].SetActive(false);
        }

        // Selecciona un nuevo minijuego que no sea el mismo que el anterior
        do
        {
            currentMinigameIndex = Random.Range(0, minigames.Length);
        } while (currentMinigameIndex == lastMinigameIndex);

        // Actualiza el índice del último minijuego
        lastMinigameIndex = currentMinigameIndex;

        // Activa el siguiente minijuego
        minigames[currentMinigameIndex].SetActive(true);

        // Reinicia el temporizador y activa el estado del minijuego
        minigameTimer = minigameDuration;
        isMinigameActive = true;
    }

    void CompleteMinigame()
    {
        isMinigameActive = false;

        // Incrementa la velocidad del juego
        gameSpeed += 0.1f;
        Time.timeScale = gameSpeed;

        // Inicia el siguiente minijuego
        StartNextMinigame();
    }

    void FailMinigame()
    {
        isMinigameActive = false;
        playerLives--;

        if (playerLives > 0)
        {
            StartNextMinigame();
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        // Muestra la pantalla de Game Over si está configurada
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        Debug.Log("Game Over");
        Time.timeScale = 1.0f; // Restablece la escala de tiempo
    }

    // Este método puede ser llamado desde los minijuegos para marcarlos como completados
    public void MarkMinigameAsComplete()
    {
        CompleteMinigame();
    }
}