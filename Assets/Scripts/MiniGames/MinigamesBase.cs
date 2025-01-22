using UnityEngine;
using UnityEngine.EventSystems;

// Clase base para todos los minijuegos, implementa varias interfaces de eventos de Unity
public class MinigamesBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    // Imagen de instrucciones para el minijuego
    public Sprite instructionImage;

    // Prefabs de efectos que pueden ser utilizados en el minijuego
    public GameObject[] effectPrefabs;

    // Método virtual para reiniciar el minijuego, puede ser sobrescrito por clases derivadas
    public virtual void ResetMinigame()
    {
        // Empty
    }

    // Método virtual que se llama cuando se detecta un clic del mouse sobre el objeto
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // Empty
    }

    // Método virtual que se llama cuando se suelta el clic del mouse sobre el objeto
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        // Empty
    }

    // Método virtual que se llama cuando se hace clic en el objeto
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        // Empty
    }

    // Método virtual que se llama cuando el puntero del mouse entra en el área del objeto
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        // Empty
    }

    // Método virtual que se llama cuando el puntero del mouse sale del área del objeto
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        // Empty
    }

    // Método que se llama cuando se arrastra el objeto con el mouse
    public virtual void OnDrag(PointerEventData eventData)
    {
        // Empty
    }
}