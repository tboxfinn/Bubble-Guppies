using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CambioAudio : MonoBehaviour
{
    //Pitch de la musica
    public AudioMixer audioMixer; 
    public Button increaseMusicPitchButton; 
    public float MusicpitchIncrement = 0.5f; 
    public float MusicmaxPitch = 3.0f; 
    private float MusiccurrentPitch = 1.0f; 

    //Pitch del SFX
    public float minSFXPitch = 0.8f;
    public float maxSFXPitch = 1.2f;

    public float randomPitchInterval = 1.0f;
    private float nextRandomChangeTime = 0f;


    void Start()
    {
        if (increaseMusicPitchButton != null)
        {
            increaseMusicPitchButton.onClick.AddListener(IncreasePitch);
        }
        else
        {
            Debug.LogError("El botón no está asignado en el Inspector.");
        }
    }


/// ///////////// Pitch del SFX en cambio ///////////////

    void Update()
    {
        if(Time.time >= nextRandomChangeTime)
        {
            RandomizeSFXPitch();
            nextRandomChangeTime = Time.time + randomPitchInterval;
        }
    }

    void RandomizeSFXPitch()
    {
        if(audioMixer != null)
        {
            float randomPitch = Random.Range(minSFXPitch, maxSFXPitch);
            audioMixer.SetFloat("SFXPitch", randomPitch);
            Debug.Log("El pitch aleatorio de SFX orita es: " + randomPitch);
        }
        else
        {
            Debug.LogError("El AudioMixer no esta puesto en el inspector");
        }
    }

/// /////////////////////////////////////////////////

    void IncreasePitch()
    {
        if (audioMixer != null)
        {
            if (MusiccurrentPitch < MusicmaxPitch)
            {
                MusiccurrentPitch += MusicpitchIncrement;
                audioMixer.SetFloat("MusicPitch", MusiccurrentPitch); 
            }
        }
        else
        {
            Debug.LogError("El AudioMixer no está asignado.");
        }
    }
}
