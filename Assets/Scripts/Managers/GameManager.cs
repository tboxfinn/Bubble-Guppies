using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }

    [Header("Minigames Settings")]
    public GameObject[] minigames;
    public float minigameDuration = 10.0f;
    public float minigameSpeedIncrease = 0.1f;
    public float minigameDurationReduction = 1.0f;
    public float minigameDurationMinimum = 4.0f;
    public int minigamesBeforeReduction = 3;
    public float delayBetweenMinigames = 3.0f;

    [Header("Game Settings")]
    public int playerLives = 3;
    public float gameSpeed = 1.0f;
    public float maxGameSpeed = 3.0f;

    [Header("UI Settings")]
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public GameObject settingsScreen;
    public TMP_Text livesText;
    public TMP_Text timerText;

    [Header("Key Bindings")]
    public KeyCode pauseKey = KeyCode.Escape;
    
    [Header("PopUps")]
    public PopUpManager popUpManager;

    [SerializeField] private GameState currentState = GameState.Playing;
    [SerializeField] private float minigameTimer;
    private bool isMinigameActive = false;
    private int currentMinigameIndex = -1;
    private int lastMinigameIndex = -1;
    private int minigamesCompleted = 0;

#region GameStart
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        void Start()
        {
            InitializeGame();
        }
    
        private void InitializeGame()
        {
            // Desactiva todos los minijuegos al inicio
            foreach (GameObject minigame in minigames)
            {
                minigame.SetActive(false);
            }
    
            Time.timeScale = gameSpeed; // Ajusta la escala de tiempo global
            
            UpdateLivesText();
    
            StartNextMinigame();
        }
    
        public void ResetGame()
        {
            // Restablece las variables del juego a sus valores iniciales
            playerLives = 3;
            gameSpeed = 1.0f;
            minigameDuration = 10.0f;
            minigamesCompleted = 0;
            currentMinigameIndex = -1;
            lastMinigameIndex = -1;
            isMinigameActive = false;
            currentState = GameState.Playing;
    
            // Desactiva todas las pantallas de UI
            if (gameOverScreen != null)
            {
                gameOverScreen.SetActive(false);
            }
            if (pauseScreen != null)
            {
                pauseScreen.SetActive(false);
            }
    
            // Desactiva todos los minijuegos
            foreach (GameObject minigame in minigames)
            {
                minigame.SetActive(false);
            }
    
            // Restablece la escala de tiempo
            Time.timeScale = gameSpeed;
    
            // Actualiza la UI
            UpdateLivesText();
    
            // Inicia el primer minijuego
            StartNextMinigame();
        }
    #endregion

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }

        if (currentState == GameState.Playing && isMinigameActive)
        {
            minigameTimer -= Time.unscaledDeltaTime;
            if (minigameTimer <= 0)
            {
                FailMinigame();
            }

            timerText.text = minigameTimer.ToString("F1");
        }
    }


    public void StartNextMinigame()
    {
        if (currentState != GameState.Playing)
            return;
        
        if (currentMinigameIndex >= 0)
        {
            // Desactiva el minijuego anterior y lo reinicia
            minigames[currentMinigameIndex].SetActive(false);
            MinigamesBase minigame = minigames[currentMinigameIndex].GetComponentInChildren<MinigamesBase>();
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

        MinigamesBase currentMinigame = minigames[currentMinigameIndex].GetComponentInChildren<MinigamesBase>();
        if (popUpManager != null && currentMinigame != null)
        {
            Debug.Log("Showing PopUp");
            popUpManager.ShowPopUp(currentMinigame.instructionImage);
        }

        StartCoroutine(StartMinigameAfterDelay(delayBetweenMinigames));
    }

    private IEnumerator StartMinigameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (popUpManager != null)
        {
            popUpManager.HidePopUp();
        }

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
        if (currentState != GameState.Playing)
            return;

        isMinigameActive = false;

        // Incrementa el contador de minijuegos completados
        minigamesCompleted++;

        // Reduce la duración del minijuego después de un cierto número de minijuegos completados
        if (minigamesCompleted % minigamesBeforeReduction == 0)
        {
            minigameDuration = Mathf.Max(minigameDurationMinimum, minigameDuration - minigameDurationReduction); // Asegura que la duración no sea menor a minigameDurationMinimum
            gameSpeed = Mathf.Min(maxGameSpeed, gameSpeed + minigameSpeedIncrease); // Incrementa la velocidad del juego
            Time.timeScale = gameSpeed; // Ajusta la escala de tiempo global
        }

        // Inicia el siguiente minijuego
        StartNextMinigame();
    }

    public void FailMinigame()
    {
        if (currentState != GameState.Playing)
            return;

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
            if (pauseScreen != null)
            {
                pauseScreen.SetActive(true);
            }
        }
        else if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = gameSpeed;
            if (pauseScreen != null)
            {
                pauseScreen.SetActive(false);
                settingsScreen.SetActive(false);
            }

        }
    }

    private void UpdateLivesText()
    {
        livesText.text = "Lives: " + playerLives;
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = gameSpeed;
            if (pauseScreen != null)
            {
                pauseScreen.SetActive(false);
            }
        }
    }

}