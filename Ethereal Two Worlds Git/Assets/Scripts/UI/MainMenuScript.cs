using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        Debug.Log("hehe");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
