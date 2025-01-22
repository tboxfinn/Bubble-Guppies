using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    public GameObject popUpPrefab; // Prefab del pop-up
    [SerializeField] private GameObject currentPopUp;
    public Transform popUpParent; // Parent del pop-up

    // Método para mostrar el pop-up con una imagen
    public void ShowPopUp(Sprite image)
    {
        if (popUpPrefab != null)
        {
            currentPopUp = Instantiate(popUpPrefab, popUpParent);
            Image popUpImage = currentPopUp.GetComponentInChildren<Image>();
            if (popUpImage != null)
            {
                popUpImage.sprite = image;
            }
        }
    }

    // Método para ocultar el pop-up
    public void HidePopUp()
    {
        if (currentPopUp != null)
        {
            Destroy(currentPopUp);
        }
    }
}
