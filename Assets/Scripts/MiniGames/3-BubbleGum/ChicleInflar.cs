using UnityEngine;
using UnityEngine.EventSystems;


public class ChicleInflar : MinigamesBase
{
    [SerializeField] private bool isGrowing = false;
    public float growthRate = 0.1f;
    public float minTargetScale = 2.0f;
    public float maxTargetScale = 5.0f;
    public float targetScale;
    public float tolerance = 0.3f;
    public GameObject guideObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetRandomTargetScale();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrowing)
        {
            transform.localScale += Vector3.one * growthRate * Time.deltaTime;
            Debug.Log("Current Scale: " + transform.localScale);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        isGrowing = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        isGrowing = false;
        CheckBubbleSize();
    }

    private void SetRandomTargetScale()
    {
        targetScale = Random.Range(minTargetScale, maxTargetScale);
        Debug.Log("Target Scale: " + targetScale);
        UpdateGuideObject();
    }

    private void UpdateGuideObject()
    {
        if (guideObject != null)
        {
            guideObject.transform.localScale = Vector3.one * targetScale;
        }
    }

    private void CheckBubbleSize()
    {
        float currentScale = transform.localScale.x; // Asumiendo que el chicle es uniforme en todas las direcciones
        if (Mathf.Abs(currentScale - targetScale) <= tolerance)
        {
            CompleteMinigame();
        }
        else
        {
            FailMinigame();
        }
    }

    private void CompleteMinigame()
    {
        Debug.Log("Minigame Completed!");
        // Aquí puedes llamar a un método en el GameManager para indicar que el minijuego se ha completado
        GameManager.instance.CompleteMinigame();
    }

    private void FailMinigame()
    {
        Debug.Log("Minigame Failed!");
        // Aquí puedes llamar a un método en el GameManager para indicar que el minijuego ha fallado
        GameManager.instance.FailMinigame();
    }

    public override void ResetMinigame()
    {
        isGrowing = false;
        transform.localScale = Vector3.one;
        SetRandomTargetScale();
    }
}
