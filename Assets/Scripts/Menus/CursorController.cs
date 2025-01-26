using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D cursor;
    public Texture2D cursorClicked;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        ChangeCursor(cursor);
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeCursor(cursorClicked);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ChangeCursor(cursor);
        }
    }

    private void ChangeCursor(Texture2D cursorType)
    {
        // Vector2 cursorHotspot = new Vector2(cursorType.width / 2, cursorType.height / 2);
        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);
    }
}
