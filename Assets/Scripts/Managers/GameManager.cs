using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Audio;
using MoreMountains.Feedbacks;

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

    [Header("UI References")]
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public GameObject settingsScreen;
    public TMP_Text livesText;
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public GameObject mainCamera;

    [Header("Key Bindings")]
    public KeyCode pauseKey = KeyCode.Escape;
    
    [Header("PopUps")]
    public PopUpManager popUpManager;

    [Header("Effects")]
    public ParticleSystem bubbleParticles;

    [Header("PitchSettings")]
    public AudioMixer audioMixer;
    private float currentPitch = 1.0f;

    [Header("Feedbacks")]
    public MMFeedbacks failedGame;
    public MMFeedbacks completedGame;
    public MMFeedbacks looseEverything;

    [SerializeField] private GameState currentState = GameState.Playing;
    [SerializeField] private float minigameTimer;
    private bool isMinigameActive = false;
    private int currentMinigameIndex = -1;
    private int lastMinigameIndex = -1;
    public int minigamesCompleted = 0;
    public int score = 0;
    private int highScore = 0;

    #region Game Manager Initialization
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
            UpdateScoreText();
            LoadHighScore();

        StartNextMinigame();
        }
    
        public void ResetGame()
        {
            // Restablece las variables del juego a sus valores iniciales
            playerLives = 3;
            score = 0;
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
            UpdateScoreText();
    
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


#region Game Start
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
                popUpManager.ShowPopUp(currentMinigame.instructionImage, currentMinigame.instructionText);
                
                bubbleParticles.Play();
                //AudioManager.instance.PlaySound(bubblePopSound);
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

            // Desactiva la cámara principal si el minijuego es PlayerNieve
            MinigamesBase currentMinigame = minigames[currentMinigameIndex].GetComponentInChildren<MinigamesBase>();
            if (currentMinigame is PlayerNieve && mainCamera != null)
            {
                mainCamera.SetActive(false);
            }
            else if (mainCamera != null)
            {
                mainCamera.SetActive(true);
            }
    
            // Reinicia el temporizador y activa el estado del minijuego
            minigameTimer = minigameDuration;
            isMinigameActive = true;
    
            // Actualiza el texto del temporizador en la UI con una décima
            timerText.text = minigameTimer.ToString("F1");
        }
    #endregion

    public void CompleteMinigame()
    {
        if (currentState != GameState.Playing)
            return;

        isMinigameActive = false;

        // Incrementa el contador de minijuegos completados
        minigamesCompleted++;

        completedGame.PlayFeedbacks();

        int scoreIncrement = Mathf.CeilToInt(10 * gameSpeed);
        score += scoreIncrement;
        UpdateScoreText();

        // Reduce la duración del minijuego después de un cierto número de minijuegos completados
        if (minigamesCompleted % minigamesBeforeReduction == 0)
        {
            // Asegura que la duración no sea menor a minigameDurationMinimum
            minigameDuration = Mathf.Max(minigameDurationMinimum, minigameDuration - minigameDurationReduction);

            // Incrementa la velocidad del juego
            gameSpeed = Mathf.Min(maxGameSpeed, gameSpeed + minigameSpeedIncrease);

            // Ajusta la escala de tiempo global
            Time.timeScale = gameSpeed;

            //aqui se aumenta el pitch del auio
            currentPitch = Mathf.Min(1.3f, currentPitch + 0.03f);  // solo va a llegar hasta 1.3 de pitch

            //para agarrar el parametro el pitch de la música
            if (audioMixer != null)
            {
                audioMixer.SetFloat("MusicPitch", currentPitch);
            }
        }

        // Comprobamos si el puntaje supera el máximo
        if (score > highScore)
        {
            highScore = score;
            SaveHighScore();  // Guardamos el nuevo puntaje máximo
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
        failedGame.PlayFeedbacks();

        UpdateLivesText();

        if (playerLives > 0)
        {
            StartNextMinigame();
        }
        else
        {
            currentPitch = 1.0f;

            if (audioMixer != null)
            {
                audioMixer.SetFloat("MusicPitch", currentPitch);
            }

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
        looseEverything.PlayFeedbacks();
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
        livesText.text = "Lives: " + "\n" + playerLives;
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + "\n" + score;
        }
    }

    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + "\n" + highScore;
        }
    }

    // Guardar el puntaje más alto
    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    // Cargar el puntaje más alto
    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);  // Default is 0 if not found
        UpdateHighScoreText();
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