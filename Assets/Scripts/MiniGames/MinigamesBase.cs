using UnityEngine;
using UnityEngine.EventSystems;

public class MinigamesBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite instructionImage;

    public virtual void ResetMinigame()
    {
        // Empty
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // Empty
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        // Empty
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        // Empty
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        // Empty
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        // Empty
    }

}
