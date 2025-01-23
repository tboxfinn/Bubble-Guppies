using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BotonContador : MinigamesBase
{
    public int playerCount = 0;
    public BubbleSpawner bubbleSpawner;

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
    }

    public void GenerateBubbles()
    {

    }

    public override void ResetMinigame()
    {
        bubbleSpawner.GenerateBubbles();
        playerCount = 0;
    }

}
