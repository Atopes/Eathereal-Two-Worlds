using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject SettingsPanel;
    public void NewGame()
    {
        SceneManager.LoadScene("StartCutscene");
    }

    public void LoadGame()
    {
        Debug.Log("hehe");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
