using MoreMountains.Feedbacks;
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
    public MMF_Player playShake;
    [SerializeField] private float shakeFrequency = 2.0f;

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
                playShake.PlayFeedbacks();
                currentShakeAmount += (shakeSpeed * Time.deltaTime) * 0.2f;
                Debug.Log("Current Shake Amount: " + currentShakeAmount);

                float shakeAmountX = Mathf.Sin(Time.time * shakeFrequency) * 0.05f; // Menos agitación en X
                float shakeAmountY = Mathf.Sin(Time.time * shakeFrequency) * 0.2f;  // Más agitación en Y
                float shakeAmountZ = Mathf.Sin(Time.time * shakeFrequency) * 0.05f; // Menos agitación en Z
                transform.localPosition = originalPosition + new Vector3(shakeAmountX, shakeAmountY, shakeAmountZ);

                float rotationAmountX = Mathf.Sin(Time.time * shakeFrequency) * 1.0f; // Menos rotación en X
                float rotationAmountY = Mathf.Sin(Time.time * shakeFrequency) * 2.0f; // Más rotación en Y
                float rotationAmountZ = Mathf.Sin(Time.time * shakeFrequency) * 5.0f; // Menos rotación en Z
                transform.localRotation = originalRotation * Quaternion.Euler(rotationAmountX, rotationAmountY, rotationAmountZ);
            }

            lastMousePosition = Input.mousePosition;

            if (currentShakeAmount >= requiredShakeAmount)
            {
                CompleteMinigame();
            }
        }
        else
        {
            // Restablece la posición y rotación original cuando no se está agitando
            playShake.StopFeedbacks();
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;
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
        transform.localRotation = originalRotation;
    }

    private void CompleteMinigame()
    {
        isShaking = false;
        Debug.Log("Minigame Completed!");
        // Aquí puedes llamar a un método en el GameManager para indicar que el minijuego se ha completado
        GameManager.instance.CompleteMinigame();
    }
}