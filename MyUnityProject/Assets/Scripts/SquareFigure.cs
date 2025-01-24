using UnityEngine;
using System.Collections.Generic;

public class SquareFigure : MonoBehaviour
{
    public List<Vector3> squarePoints;

    void Start()
    {
        DefineSquarePoints();
    }

    void DefineSquarePoints()
    {
        float size = 1.0f; // Size of the square
        squarePoints = new List<Vector3>
        {
            new Vector3(-size / 2, 0, -size / 2),
            new Vector3(size / 2, 0, -size / 2),
            new Vector3(size / 2, 0, size / 2),
            new Vector3(-size / 2, 0, size / 2)
        };
    }
}