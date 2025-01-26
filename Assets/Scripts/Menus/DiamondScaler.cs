using UnityEngine;

public class DiamondScaler : MonoBehaviour
{
    public RectTransform[] diamonds; // Array de rombos en la UI
    public float minScale = 0.5f;    // Escala mínima
    public float maxScale = 1.5f;    // Escala máxima
    public float speed = 1.0f;       // Velocidad de oscilación

    private float[] offsets;         // Desplazamientos individuales para variar el ciclo

    private void Start()
    {
        // Inicializar desplazamientos aleatorios para cada rombo
        offsets = new float[diamonds.Length];
        for (int i = 0; i < diamonds.Length; i++)
        {
            offsets[i] = Random.Range(0f, Mathf.PI * 2); // Aleatorio en el ciclo seno
        }
    }

    private void Update()
    {
        for (int i = 0; i < diamonds.Length; i++)
        {
            // Calcula la escala usando el seno
            float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * speed + offsets[i]) + 1) / 2);
            diamonds[i].localScale = new Vector3(scale, scale, 1);
        }
    }
}
