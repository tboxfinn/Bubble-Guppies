using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CambioAudio : MonoBehaviour
{
    public AudioMixer audioMixer; 
    public Button increasePitchButton; 
    public float pitchIncrement = 0.5f; 
    public float maxPitch = 3.0f; 
    private float currentPitch = 1.0f; 

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
        if (audioMixer != null)
        {
            if (currentPitch < maxPitch)
            {
                currentPitch += pitchIncrement;
                audioMixer.SetFloat("MusicPitch", currentPitch); 
            }
        }
        else
        {
            Debug.LogError("El AudioMixer no está asignado.");
        }
    }
}
