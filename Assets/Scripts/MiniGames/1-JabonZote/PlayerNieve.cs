using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlayerNieve : MinigamesBase
{
    public List<Vector3> figurePoints; // Lista de puntos que definen la figura
    private int currentPointIndex = 0;
    [SerializeField] private bool isDrawing = false;
    [SerializeField] private float tolerance = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDrawing)
        {
            MovePlayer();
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        isDrawing = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        isDrawing = false;
        CheckCompletion();
    }

    void MovePlayer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.y = 0; // Asegúrate de que el jugador se mueva solo en el plano XZ

        transform.position = new Vector3(mousePosition.x, 0, mousePosition.z);

        if (Vector3.Distance(transform.position, figurePoints[currentPointIndex]) < 0.1f)
        {
            currentPointIndex++;
            if (currentPointIndex >= figurePoints.Count)
            {
                currentPointIndex = 0;
            }
        }
        else if (!IsWithinTolerance(mousePosition))
        {
            Debug.Log("Te has salido de los límites!");
            isDrawing = false;
            currentPointIndex = 0; // Reinicia el índice de puntos
        }
    }

    bool IsWithinTolerance(Vector3 position)
    {
        foreach (var point in figurePoints)
        {
            if (Vector3.Distance(position, point) <= tolerance)
            {
                return true;
            }
        }
        return false;
    }

    void CheckCompletion()
    {
        if (currentPointIndex == figurePoints.Count - 1)
        {
            Debug.Log("Figura completada!");
            // Lógica para cuando el jugador completa la figura
        }
        else
        {
            Debug.Log("Figura no completada. Inténtalo de nuevo.");
            // Lógica para cuando el jugador no completa la figura
        }

        currentPointIndex = 0; // Reinicia el índice de puntos
    }
}
