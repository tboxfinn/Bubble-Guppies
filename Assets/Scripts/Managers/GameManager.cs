using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    [Header("Minigames Settings")]
    public GameObject[] minigames;
    public float minigameDuration = 10.0f;
    public float minigameSpeedIncrease = 0.1f;
    public float minigameDurationReduction = 1.0f;
    public int minigamesBeforeReduction = 3;
    public float minigameDurationMinimum = 4.0f;

    [Header("Game Settings")]
    public int playerLives = 3;
    public float gameSpeed = 1.0f;

    [Header("UI Settings")]
    public GameObject gameOverScreen;
    public TMP_Text livesText;
    public TMP_Text timerText;
    
    [SerializeField] private GameState currentState = GameState.Playing;
    [SerializeField] private float minigameTimer;
    private bool isMinigameActive = false;
    private int currentMinigameIndex = -1;
    private int lastMinigameIndex = -1;
    private int minigamesCompleted = 0;

    private void Awake()
    {
        // Implementación del patrón singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: Mantener el GameManager entre escenas
        }
        else
        {
            Destroy(gameObject);
        }

        // Establece la escala de tiempo inicial
        Time.timeScale = gameSpeed;
    }

    void Start()
    {
        // Desactiva todos los minijuegos al inicio
        foreach (GameObject minigame in minigames)
        {
            minigame.SetActive(false);
        }
        
        UpdateLivesText();

        StartNextMinigame();
    }

    void Update()
    {
        if (currentState == GameState.Playing && isMinigameActive)
        {
            // Actualiza el temporizador del minijuego usando Time.unscaledDeltaTime
            minigameTimer -= Time.unscaledDeltaTime;
            if (minigameTimer <= 0)
            {
                FailMinigame();
            }

            // Actualiza el texto del temporizador en la UI con una décima
            timerText.text = minigameTimer.ToString("F1");
        }
    }

    public void StartNextMinigame()
    {
        if (currentMinigameIndex >= 0)
        {
            // Desactiva el minijuego anterior y lo reinicia
            minigames[currentMinigameIndex].SetActive(false);
            IMinigame minigame = minigames[currentMinigameIndex].GetComponentInChildren<IMinigame>();
            if (minigame != null)
            {
                minigame.ResetMinigame();
            }
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

        // Actualiza el texto del temporizador en la UI con una décima
        timerText.text = minigameTimer.ToString("F1");
    }

    public void CompleteMinigame()
    {
        isMinigameActive = false;

        // Incrementa el contador de minijuegos completados
        minigamesCompleted++;

        // Reduce la duración del minijuego después de un cierto número de minijuegos completados
        if (minigamesCompleted % minigamesBeforeReduction == 0)
        {
            minigameDuration = Mathf.Max(minigameDurationMinimum, minigameDuration - minigameDurationReduction); // Asegura que la duración no sea menor a minigameDurationMinimum
            gameSpeed += minigameSpeedIncrease; // Incrementa la velocidad del juego
            Time.timeScale = gameSpeed; // Ajusta la escala de tiempo global
        }

        // Inicia el siguiente minijuego
        StartNextMinigame();
    }

    public void FailMinigame()
    {
        isMinigameActive = false;
        playerLives--;

        UpdateLivesText();

        if (playerLives > 0)
        {
            StartNextMinigame();
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        currentState = GameState.GameOver;

        // Muestra la pantalla de Game Over si está configurada
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        Debug.Log("Game Over");
        Time.timeScale = 1.0f; // Restablece la escala de tiempo
    }

    // Este método puede ser llamado desde los minijuegos para marcarlos como completados
    public void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0;
        }
        else if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = gameSpeed;
        }
    }

    private void UpdateLivesText()
    {
        livesText.text = "Lives: " + playerLives;
    }
}