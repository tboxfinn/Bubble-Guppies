using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Bubble : MonoBehaviour, IPointerClickHandler
{
    public bool isPopped = false;
    public GameObject inflatedBubble;
    public GameObject poppedBubble;

    public event Action OnBubblePopped;

    void Start()
    {
        UpdateBubble();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isPopped)
        {
            isPopped = true;
            UpdateBubble();
            OnBubblePopped?.Invoke();
            // Lógica adicional para cuando una burbuja se revienta
        }
    }

    public void UpdateBubble()
    {
        if (isPopped)
        {
            inflatedBubble.SetActive(false);
            poppedBubble.SetActive(true);
        }
        else
        {
            inflatedBubble.SetActive(true);
            poppedBubble.SetActive(false);
        }
    }
}