using UnityEngine;
using UnityEngine.EventSystems;

public class ShakeAgitar : MinigamesBase
{
    [SerializeField] private bool isShaking = false;
    [SerializeField] private float shakeThreshold = 10.0f; // Umbral de velocidad de agitación
     public float requiredShakeAmount = 4000.0f; // Cantidad de agitación requerida para completar el minijuego
    [SerializeField] private float currentShakeAmount = 0.0f;
    private Vector3 lastMousePosition;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShaking)
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            float shakeSpeed = mouseDelta.magnitude / Time.deltaTime;

            if (shakeSpeed > shakeThreshold)
            {
                currentShakeAmount += (shakeSpeed * Time.deltaTime) * 0.2f;
                Debug.Log("Current Shake Amount: " + currentShakeAmount);

                float shakeAmount = Mathf.Sin(Time.time * shakeSpeed) * 0.1f;
                transform.localPosition = originalPosition + new Vector3(shakeAmount, shakeAmount, 0.0f);

                float rotationAmount = Mathf.Sin(Time.time * shakeSpeed) * 5.0f;
                transform.localRotation = originalRotation * Quaternion.Euler(0.0f, 0.0f, rotationAmount);
            }

            lastMousePosition = Input.mousePosition;

            if (currentShakeAmount >= requiredShakeAmount)
            {
                CompleteMinigame();
            }
        }
        else
        {
            // Restablece la posición original cuando no se está agitando
            transform.localPosition = originalPosition;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        isShaking = true;
        lastMousePosition = Input.mousePosition;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        isShaking = false;
        currentShakeAmount = 0.0f; // Reinicia la cantidad de agitación si el jugador suelta el objeto
    }

    public override void ResetMinigame()
    {
        isShaking = false;
        currentShakeAmount = 0.0f;
        transform.localPosition = originalPosition;
    }

    private void CompleteMinigame()
    {
        isShaking = false;
        Debug.Log("Minigame Completed!");
        // Aquí puedes llamar a un método en el GameManager para indicar que el minijuego se ha completado
        GameManager.instance.CompleteMinigame();
    }
}