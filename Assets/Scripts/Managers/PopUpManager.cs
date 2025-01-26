using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    public GameObject popUpPrefab; // Prefab del pop-up
    public TMP_Text instructionText; // Texto de instrucciones
    [SerializeField] private GameObject currentPopUp;
    public Transform popUpParent; // Parent del pop-up
    public GameObject colorBackground; // Fondo de color aleatorio

    // PALETA DE COLORES NO TOCAR
    private Color[] colors = new Color[]
    {
        ColorUtility.TryParseHtmlString("#9B5DE5", out var color1) ? color1 : Color.white,
        ColorUtility.TryParseHtmlString("#F15BB5", out var color2) ? color2 : Color.white,
        ColorUtility.TryParseHtmlString("#FEE440", out var color3) ? color3 : Color.white,
        ColorUtility.TryParseHtmlString("#00BBF9", out var color4) ? color4 : Color.white,
        ColorUtility.TryParseHtmlString("#00F5D4", out var color5) ? color5 : Color.white,
        ColorUtility.TryParseHtmlString("#FF9770", out var color6) ? color6 : Color.white,
        ColorUtility.TryParseHtmlString("#FFD670", out var color7) ? color7 : Color.white,
        ColorUtility.TryParseHtmlString("#AFFC41", out var color8) ? color8 : Color.white,
        ColorUtility.TryParseHtmlString("#3C1642", out var color9) ? color9 : Color.white
    };

    // Método para mostrar el pop-up con una imagen
    public void ShowPopUp(Sprite[] images, string instructionText)
    {
        if (popUpPrefab != null)
        {
            currentPopUp = Instantiate(popUpPrefab, popUpParent);
            // Image popUpImage = currentPopUp.GetComponentInChildren<Image>();
            UISpriteAnimation spriteAnimation = currentPopUp.GetComponentInChildren<UISpriteAnimation>();
            TMP_Text[] popUpTexts = currentPopUp.GetComponentsInChildren<TMP_Text>();
            

            // if (popUpImage != null)
            // {
            //     popUpImage.sprite = image;
            // }
            if (spriteAnimation != null)
            {
                spriteAnimation.SetSprites(images);
            }
            if (instructionText != null)
            {
                foreach (TMP_Text text in popUpTexts)
                {
                    text.text = instructionText;
                }
            }

            // Cambio del color del fondo

            Color randomColor = colors[Random.Range(0, colors.Length)];

            if (colorBackground != null)
            {
                colorBackground.SetActive(true); // prender el panel
                Image backgroundImage = colorBackground.GetComponent<Image>();
                if (backgroundImage != null)
                {
                    backgroundImage.color = randomColor; // vambiara a un color random
                }
            }
        }
    }

    // Método para ocultar el pop-up y el fondo
    public void HidePopUp()
    {
        if (currentPopUp != null)
        {
            Destroy(currentPopUp);
        }

        if (colorBackground != null)
        {
            colorBackground.SetActive(false); // Desactivar el panel
        }
    }
}
