using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Feedbacks;

public class WriteBubble : MinigamesBase
{
    public MMF_Player playTecla;
    public MMF_Player playSaltoLinea;
    public TMP_Text bubbleText;
    public int keyPressCount = 0;
    public int requiredKeyPresses = 50;
    private string playerInput = "";
    private int maxCharsPerLine = 20;
    [SerializeField] private int maxRequiredKeyPresses = 150;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AdjustDifficulty();
        bubbleText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        foreach (char c in Input.inputString)
        {
            playerInput += c;
            keyPressCount++;
            playTecla.PlayFeedbacks();
            // Insert a newline character if the current line exceeds maxCharsPerLine
            if (playerInput.Length % maxCharsPerLine == 0)
            {
                playerInput += "\n";
                playSaltoLinea.PlayFeedbacks();
            }

            bubbleText.text = playerInput;

            if (keyPressCount >= requiredKeyPresses)
            {
                GameManager.instance.CompleteMinigame();
            }
        }
    }

    public override void ResetMinigame()
    {
        keyPressCount = 0;
        playerInput = "";
        bubbleText.text = "";
        AdjustDifficulty();
    }

    private void AdjustDifficulty()
    {
        float gameSpeed = GameManager.instance.gameSpeed;
        requiredKeyPresses = Mathf.CeilToInt(requiredKeyPresses * Mathf.Sqrt(gameSpeed)); // Ajusta la dificultad en función de la raíz cuadrada de gameSpeed
        requiredKeyPresses = Mathf.CeilToInt(requiredKeyPresses * Mathf.Sqrt(gameSpeed));
        requiredKeyPresses = Mathf.Min(requiredKeyPresses, maxRequiredKeyPresses); // Limita el valor máximo
    }
}
