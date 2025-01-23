using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    public GameObject popUpPrefab; // Prefab del pop-up
    public TMP_Text instructionText; // Texto de instrucciones
    [SerializeField] private GameObject currentPopUp;
    public Transform popUpParent; // Parent del pop-up

    // Método para mostrar el pop-up con una imagen
    public void ShowPopUp(Sprite image, string instructionText)
    {
        if (popUpPrefab != null)
        {
            currentPopUp = Instantiate(popUpPrefab, popUpParent);
            Image popUpImage = currentPopUp.GetComponentInChildren<Image>();
            TMP_Text popUpInstructionText = currentPopUp.GetComponentInChildren<TMP_Text>();

            if (popUpImage != null)
            {
                popUpImage.sprite = image;
            }
            if (instructionText != null)
            {
                popUpInstructionText.text = instructionText;
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
