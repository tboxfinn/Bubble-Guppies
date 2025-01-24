using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CircleFigure : MinigamesBase
{
    public List<Vector3> circlePoints; // Lista de puntos que definen el círculo
    private int currentPointIndex = 0;
    [SerializeField] private float radius = 5f; // Radio del círculo
    [SerializeField] private int pointCount = 100; // Número de puntos que definen el círculo
    [SerializeField] private bool isDrawing = false;

    void Start()
    {
        GenerateCirclePoints();
    }

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

    void GenerateCirclePoints()
    {
        circlePoints = new List<Vector3>();
        for (int i = 0; i < pointCount; i++)
        {
            float angle = i * Mathf.PI * 2 / pointCount;
            Vector3 point = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            circlePoints.Add(point);
        }
    }

    void MovePlayer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.y = 0;

        transform.position = new Vector3(mousePosition.x, 0, mousePosition.z);

        if (Vector3.Distance(transform.position, circlePoints[currentPointIndex]) < 0.1f)
        {
            currentPointIndex++;
            if (currentPointIndex >= circlePoints.Count)
            {
                currentPointIndex = 0;
            }
        }
        else if (!IsWithinTolerance(mousePosition))
        {
            Debug.Log("Te has salido de los límites!");
            isDrawing = false;
            currentPointIndex = 0;
        }
    }

    bool IsWithinTolerance(Vector3 position)
    {
        foreach (var point in circlePoints)
        {
            if (Vector3.Distance(position, point) <= 0.5f) // Tolerancia fija
            {
                return true;
            }
        }
        return false;
    }

    void CheckCompletion()
    {
        if (currentPointIndex == circlePoints.Count - 1)
        {
            Debug.Log("Círculo completado!");
        }
        else
        {
            Debug.Log("Círculo no completado. Inténtalo de nuevo.");
        }

        currentPointIndex = 0; // Reinicia el índice de puntos
    }
}