using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BotonContador : MinigamesBase
{
    public GameObject bubblePrefab;
    public int numberOfBubbles = 20;
    public List<Color> bubbleColors;
    public Button countButton;
    public Color selectedColor;
    private List<GameObject> bubbles = new List<GameObject>();
    private int correctCount = 0;
    private int playerCount = 0;

    void Start()
    {
        GenerateBubbles();
        countButton.onClick.AddListener(CountBubbles);
        correctCount = CountBubblesOfSelectedColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateBubbles()
    {
        for (int i = 0; i < numberOfBubbles; i++)
        {
            GameObject bubble = Instantiate(bubblePrefab, GetRandomPosition(), Quaternion.identity);
            Color randomColor = bubbleColors[Random.Range(0, bubbleColors.Count)];
            bubble.GetComponent<SpriteRenderer>().color = randomColor;
            bubbles.Add(bubble);
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-5f, 5f);
        float y = Random.Range(-5f, 5f);
        return new Vector3(x, y, 0);
    }

    void CountBubbles()
    {
        playerCount++;
        if (playerCount == correctCount)
        {
            GameManager.instance.CompleteMinigame();
        }
        else if (playerCount > correctCount)
        {
            GameManager.instance.FailMinigame();
        }
    }

    int CountBubblesOfSelectedColor()
    {
        int count = 0;
        foreach (GameObject bubble in bubbles)
        {
            if (bubble.GetComponent<SpriteRenderer>().color == selectedColor)
            {
                count++;
            }
        }
        return count;
    }
}
