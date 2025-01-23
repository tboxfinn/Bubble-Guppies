using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BotonContador : MinigamesBase
{
    public GameObject bubblePrefab;
    public int bubblesToGenerate = 10;
    public List<Material> bubbleMaterials;
    public Material selectedMaterial;
    public GameObject bubbleContainer;
    public int playerCount = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        playerCount++;
    }

    void GenerateBubbles()
    {
        for (int i = 0; i < bubblesToGenerate; i++)
        {
            GameObject bubble = Instantiate(bubblePrefab, bubbleContainer.transform);
            Material randomMaterial = bubbleMaterials[Random.Range(0, bubbleMaterials.Count)];
            bubble.GetComponent<Renderer>().material = randomMaterial;
        }
    }

    public override void ResetMinigame()
    {
        playerCount = 0;
        GenerateBubbles();
    }

}
