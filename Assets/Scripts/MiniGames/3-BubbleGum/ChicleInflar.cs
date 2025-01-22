using UnityEngine;
using UnityEngine.EventSystems;


public class ChicleInflar : MinigamesBase
{
    [SerializeField] private bool isGrowing = false;
    public float growthRate = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
    }

    public override void ResetMinigame()
    {
        isGrowing = false;
        transform.localScale = Vector3.one;
    }
}
