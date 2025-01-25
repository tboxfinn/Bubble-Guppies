using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopoteSubir : MinigamesBase
{
    [SerializeField] private int baseRequiredSwipes = 20;
    [SerializeField] private float swipeThreshold = 75.0f; // Umbral de distancia para considerar un deslizamiento
    [SerializeField] private int requiredSwipes;
    [SerializeField] private int currentSwipes = 0;
    private Vector2 startTouchPosition;
    [SerializeField] private bool isSwiping = false;
    public GameObject bolitasSubiendo;

    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;

    public MMF_Player playSubir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AdjustDifficulty();
        ResetBolitasPosition();
    }

    void Update()
    {

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        startTouchPosition = Input.mousePosition;
        isSwiping = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        isSwiping = false;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (isSwiping)
        {
            Vector2 currentTouchPosition = Input.mousePosition;
            float swipeDistance = currentTouchPosition.y - startTouchPosition.y;

            if (swipeDistance > swipeThreshold)
            {
                currentSwipes++;
                Debug.Log("Current Swipes: " + currentSwipes);
                playSubir.PlayFeedbacks();
                UpdateBolitasPosition();

                if (currentSwipes >= requiredSwipes)
                {
                    CompleteMinigame();
                }

                // Reiniciar la posición inicial para el próximo deslizamiento
                startTouchPosition = currentTouchPosition;
            }
        }
        else
        {
            playSubir.StopFeedbacks();
        }
    }

    private void UpdateBolitasPosition()
    {
        if (bolitasSubiendo != null)
        {
            float progress = (float)currentSwipes / requiredSwipes;
            bolitasSubiendo.transform.position = Vector3.Lerp(startPosition.position, endPosition.position, progress);
        }
    }

    private void ResetBolitasPosition()
    {
        if (bolitasSubiendo != null)
        {
            bolitasSubiendo.transform.position = startPosition.position;
        }
    }

    private void CompleteMinigame()
    {
        Debug.Log("Minigame Completed!");
        // Aquí puedes llamar a un método en el GameManager para indicar que el minijuego se ha completado
        GameManager.instance.CompleteMinigame();
    }

    public override void ResetMinigame()
    {
        currentSwipes = 0;
        isSwiping = false;
        ResetBolitasPosition();
    }

    private void AdjustDifficulty()
    {
        // Accede a gameSpeed desde GameManager
        float gameSpeed = GameManager.instance.gameSpeed;

        // Ajusta requiredSwipes y swipeThreshold en función de gameSpeed
        requiredSwipes = Mathf.CeilToInt(baseRequiredSwipes * gameSpeed);

        Debug.Log("Adjusted Difficulty - Required Swipes: " + requiredSwipes + ", Swipe Threshold: " + swipeThreshold);
    }
}
