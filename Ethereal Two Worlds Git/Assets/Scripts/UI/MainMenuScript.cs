using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void NewGame()
    {
        ResetPrefs();
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

    private void ResetPrefs()
    {
        string[] keys = new string[5] { PlayerPrefs.GetString("Interact"), PlayerPrefs.GetString("Jump"),
            PlayerPrefs.GetString("Dash"), PlayerPrefs.GetString("Attack"), PlayerPrefs.GetString("Cast") };
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("Interact", keys[0]);
        PlayerPrefs.SetString("Jump", keys[1]);
        PlayerPrefs.SetString("Dash", keys[2]);
        PlayerPrefs.SetString("Attack", keys[3]);
        PlayerPrefs.SetString("Cast", keys[4]);
        PlayerPrefs.SetInt("MaxHealth", 3);
        PlayerPrefs.SetInt("CurrHP", 3);
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("MeleeDmg", 3);
        PlayerPrefs.SetFloat("RespawnX", 0f);
        PlayerPrefs.SetFloat("RespawnY", -3.5f);
        PlayerPrefs.Save();
    }
}
