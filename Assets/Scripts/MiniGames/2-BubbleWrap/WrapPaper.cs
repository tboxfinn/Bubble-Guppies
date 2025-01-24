using UnityEngine;
using System.Collections.Generic;

public class WrapPaper : MinigamesBase
{
    public GameObject bubblePrefab;
    public int rows = 5;
    public int columns = 5;
    public float spacing = 1.5f;

    private List<Bubble> bubbles = new List<Bubble>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateBubbles();
    }

    void GenerateBubbles()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = new Vector3(i * spacing, j * spacing, 0);
                GameObject bubbleObject = Instantiate(bubblePrefab, position, Quaternion.identity);
                bubbleObject.transform.SetParent(transform);

                Bubble bubble = bubbleObject.GetComponent<Bubble>();
                if (bubble != null)
                {
                    bubble.OnBubblePopped += HandleBubblePopped;
                    bubbles.Add(bubble);
                }
            }
        }
    }

    void HandleBubblePopped()
    {
        if (AllBubblesPopped())
        {
            GameManager.instance.CompleteMinigame();
        }
    }

    bool AllBubblesPopped()
    {
        foreach (Bubble bubble in bubbles)
        {
            if (!bubble.isPopped)
            {
                return false;
            }
        }
        return true;
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