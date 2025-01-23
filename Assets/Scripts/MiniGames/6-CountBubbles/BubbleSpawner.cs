using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class BubbleSpawner : MinigamesBase
{
    [Header("References")]
    public GameObject bubblePrefab;
    public List<Material> bubbleMaterials;
    public GameObject bubbleContainer;
    public GameObject bubbleMuestra;
    public TMP_Text playerCountText;

    [Header("Settings")]
    public int bubblesToGenerate = 10;
    public int maxBubblesToGenerate = 25;
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;

    [Header("Debug")]
    public int playerCount = 0;
    public Material selectedMaterial;
    private int selectedMaterialCount = 0;
    private bool isCheckingPlayerCount = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetMinigame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        playerCount++;
        UpdatePlayerCountText();
        if (!isCheckingPlayerCount)
        {
            StartCoroutine(CheckPlayerCountWithDelay());
        }
    }

    public void GenerateBubbles()
    {
        if (bubbleContainer == null)
        {
            Debug.LogError("Bubble Container is not assigned!");
            return;
        }

        selectedMaterialCount = 0;
        selectedMaterial = bubbleMaterials[Random.Range(0, bubbleMaterials.Count)];

        bubbleMuestra.GetComponent<Renderer>().material = selectedMaterial;

        // Ensure at least one bubble has the selected material
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            Random.Range(spawnAreaMin.z, spawnAreaMax.z)
        );

        GameObject firstBubble = Instantiate(bubblePrefab, randomPosition, Quaternion.identity, bubbleContainer.transform);
        firstBubble.GetComponent<Renderer>().material = selectedMaterial;
        selectedMaterialCount++;

        // Generate the rest of the bubbles
        for (int i = 1; i < bubblesToGenerate; i++)
        {
            randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            GameObject bubble = Instantiate(bubblePrefab, randomPosition, Quaternion.identity, bubbleContainer.transform);
            Material randomMaterial = bubbleMaterials[Random.Range(0, bubbleMaterials.Count)];
            bubble.GetComponent<Renderer>().material = randomMaterial;

            if (randomMaterial == selectedMaterial)
            {
                selectedMaterialCount++;
            }
        }

        Debug.Log($"Selected Material: {selectedMaterial.name}, Count: {selectedMaterialCount}");
    }


    public override void ResetMinigame()
    {
        if (bubbleContainer == null)
        {
            Debug.LogError("Bubble Container is not assigned!");
            return;
        }

        // Clear existing bubbles
        foreach (Transform child in bubbleContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Reset player count
        playerCount = 0;
        UpdatePlayerCountText();

        // Generate new bubbles
        GenerateBubbles();
    }

    private IEnumerator CheckPlayerCountWithDelay()
    {
        isCheckingPlayerCount = true;
        yield return new WaitForSeconds(0.75f);
        CheckPlayerCount();
        isCheckingPlayerCount = false;
    }

    private void CheckPlayerCount()
    {
        if (playerCount == selectedMaterialCount)
        {
           GameManager.instance.CompleteMinigame();
           OnMinigameCompleted();
        }
        else if (playerCount > selectedMaterialCount)
        {
            GameManager.instance.FailMinigame();
        }
    }

    private void UpdatePlayerCountText()
    {
        if (playerCountText != null)
        {
            playerCountText.text = $"Count: \n {playerCount}";
        }
        else
        {
            Debug.LogError("Player Count Text is not assigned!");
        }
    }

    private void OnMinigameCompleted()
    {
        if (GameManager.instance.minigamesCompleted % GameManager.instance.minigamesBeforeReduction == 0)
        {
            IncreaseBubblesToGenerate();
        }
    }

    private void IncreaseBubblesToGenerate()
    {
        if (bubblesToGenerate + 2 <= maxBubblesToGenerate)
        {
            bubblesToGenerate += 2;
        }
        else
        {
            bubblesToGenerate = maxBubblesToGenerate;
        }
        ResetMinigame();
    }
}
