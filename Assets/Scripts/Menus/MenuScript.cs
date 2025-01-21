using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
   public string Game;
   public void LoadNewSceneMENU()
   {
        SceneManager.LoadScene(Game);
   }

    public void QuitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}