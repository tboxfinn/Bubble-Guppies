using UnityEngine;

public class ChicleInflar : MonoBehaviour, IMinigame
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

    void OnMouseDown()
    {
        isGrowing = true;
    }

    void OnMouseUp()
    {
        isGrowing = false;
    }

    public void ResetMinigame()
    {
        isGrowing = false;
        transform.localScale = Vector3.one;
    }
}
