using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class WriteBubble : MinigamesBase
{
    public TMP_Text bubbleText;
    public int keyPressCount = 0;
    public int requiredKeyPresses = 100;
    private string playerInput = "";
    private int maxCharsPerLine = 33;
    private int maxRequiredKeyPresses = 275;

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

            // Insert a newline character if the current line exceeds maxCharsPerLine
            if (playerInput.Length % maxCharsPerLine == 0)
            {
                playerInput += "\n";
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
