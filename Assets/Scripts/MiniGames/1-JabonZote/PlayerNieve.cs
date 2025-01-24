using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlayerNieve : MinigamesBase
{
    [Header("Figuras")]
    public List<Vector3> squarePoints;
    public List<Vector3> circlePoints;
    public List<Vector3> trianglePoints;
    private List<Vector3> figurePoints;

    [SerializeField] private int currentPointIndex = 0;
    [SerializeField] private bool isDrawing = false;
    [SerializeField] private float tolerance = 1f;
    [SerializeField] private LineRenderer lineRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SelectRandomFigure();
        DrawPath();
        TeleportToStart();
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
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.y;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.y = 0; // Asegúrate de que el jugador se mueva solo en el plano XZ

        transform.position = new Vector3(worldPosition.x, 0, worldPosition.z);

        Debug.Log("Posición del jugador: " + transform.position);
        Debug.Log("Posición del punto actual: " + figurePoints[currentPointIndex]);

        if (Vector3.Distance(transform.position, figurePoints[currentPointIndex]) < tolerance)
        {
            Debug.Log("Punto alcanzado: " + currentPointIndex);
            currentPointIndex++;
            if (currentPointIndex >= figurePoints.Count)
            {
                Debug.Log("Todos los puntos alcanzados!");
                isDrawing = false;
                CheckCompletion();
            }
        }
    }

    void CheckCompletion()
    {
        if (currentPointIndex == figurePoints.Count)
        {
            Debug.Log("Figura completada!");
            // Lógica para cuando el jugador completa la figura
            GameManager.instance.CompleteMinigame();
        }
        else
        {
            Debug.Log("Figura no completada. Inténtalo de nuevo.");
            // Lógica para cuando el jugador no completa la figura
            GameManager.instance.FailMinigame();
        }

        currentPointIndex = 0; // Reinicia el índice de puntos
    }

    void SelectRandomFigure()
    {
        int randomFigure = Random.Range(0, 3); // Selecciona un número al azar entre 0 y 2
        if (randomFigure == 0)
        {
            figurePoints = squarePoints;
            Debug.Log("Figura seleccionada: Cuadrado");
        }
        else if (randomFigure == 1)
        {
            figurePoints = circlePoints;
            Debug.Log("Figura seleccionada: Círculo");
        }
        else
        {
            figurePoints = trianglePoints;
            Debug.Log("Figura seleccionada: Triángulo");
        }

        Debug.Log("Puntos de la figura seleccionada: " + figurePoints.Count);
    }

    void DrawPath()
    {
        if (lineRenderer != null && figurePoints != null)
        {
            List<Vector3> pointsToDraw = new List<Vector3>(figurePoints);
            if (pointsToDraw.Count > 0)
            {
                pointsToDraw.Add(pointsToDraw[0]); // Añadir el primer punto al final para cerrar el bucle
            }
            lineRenderer.positionCount = pointsToDraw.Count;
            lineRenderer.SetPositions(pointsToDraw.ToArray());
        }

    }

    void TeleportToStart()
    {
        if (figurePoints != null && figurePoints.Count > 0)
        {
            Vector3 startPoint = figurePoints[0];
            transform.position = new Vector3(startPoint.x, 0, startPoint.z);
        }
    }

    public override void ResetMinigame()
    {
        currentPointIndex = 0;
        isDrawing = false;
        SelectRandomFigure();
        DrawPath();
        TeleportToStart();
    }
}