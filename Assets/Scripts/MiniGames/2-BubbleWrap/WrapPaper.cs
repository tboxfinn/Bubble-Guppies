using UnityEngine;
using System.Collections.Generic;

public class WrapPaper : MinigamesBase
{
    public GameObject bubblePrefab;
    public int rows = 5;
    public int columns = 5;
    public float spacingX = 1.5f;
    public float spacingY = 1.5f;
    public float initialPoppedProbability = 0.9f; // Probabilidad de que una burbuja aparezca reventada
    public float difficultyIncreaseRate = 0.05f;
    public float minPoppedProbability = 0.4f;

    private List<Bubble> bubbles = new List<Bubble>();
    private int bubblesToPop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateBubbles();
    }

    void GenerateBubbles()
    {
        float startX = -(columns - 1) * spacingX / 2;
        float startY = -(rows - 1) * spacingY / 2;

        bubblesToPop = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = new Vector3(startX + j * spacingX, startY + i * spacingY, 0);
                GameObject bubbleObject = Instantiate(bubblePrefab, transform);
                bubbleObject.transform.localPosition = position;
                bubbleObject.transform.localRotation = Quaternion.identity;

                Bubble bubble = bubbleObject.GetComponent<Bubble>();
                if (bubble != null)
                {
                    // Establecer aleatoriamente el estado de la burbuja
                    bubble.isPopped = Random.value < initialPoppedProbability;
                    bubble.UpdateBubble();

                    if (!bubble.isPopped)
                    {
                        bubblesToPop++;
                    }

                    bubble.OnBubblePopped += HandleBubblePopped;
                    bubbles.Add(bubble);
                }
            }
        }
    }

    void HandleBubblePopped()
    {
        bubblesToPop--;

        if (bubblesToPop <= 0)
        {
            IncreaseDifficulty();
            GameManager.instance.CompleteMinigame();
        }
    }

    void IncreaseDifficulty()
    {
        initialPoppedProbability = Mathf.Max(minPoppedProbability, initialPoppedProbability - difficultyIncreaseRate);
    }

    public override void ResetMinigame()
    {
        // Destruir burbujas existentes
        foreach (Bubble bubble in bubbles)
        {
            if (bubble != null)
            {
                Destroy(bubble.gameObject);
            }
        }
        bubbles.Clear();

        // Generar nuevas burbujas
        GenerateBubbles();
    }

    
}