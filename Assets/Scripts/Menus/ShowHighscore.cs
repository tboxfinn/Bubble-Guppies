using UnityEngine;
using TMPro;

public class ShowHighScore : MonoBehaviour
{
    public TMP_Text highScoreText;  // pa agarrar el texto donde va el highscore

    void Start()
    {
        // cargar el highscore de los prefs
        int highScore = PlayerPrefs.GetInt("HighScore", 0);  // Default es 0 si no existe

        // mostrarlo en el text del canvas
        if (highScoreText != null)
        {
            highScoreText.text = "HighScore: " + "\n" + highScore.ToString();
        }
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        Debug.Log("HighScore reset to 0");
    }
}
