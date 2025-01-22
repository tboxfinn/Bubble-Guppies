using UnityEngine;
using UnityEngine.UI;

public class CambioAudiio : MonoBehaviour
{


    public AudioSource audioSource;
    public Button increasePitchButton;
    public float pitchIncrement = 0.5f;
    public float maxPitch = 3.0f;

    void Start()
    {
        if (increasePitchButton != null)
        {
            increasePitchButton.onClick.AddListener(IncreasePitch);
        }
        else
        {
            Debug.LogError("El botón no está asignado en el Inspector.");
        }
    }

    void IncreasePitch()
    {
        if (audioSource != null && audioSource.pitch < maxPitch)
        {
            audioSource.pitch += pitchIncrement;
        }
    }

}
