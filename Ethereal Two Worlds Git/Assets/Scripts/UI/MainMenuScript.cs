using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI OnOffText, ResolutionLabel;
    [SerializeField]
    private TMP_Dropdown ResolutionDropdown;

    private bool isFullscreen;
    private int width, height;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Width"))
        {
            width = PlayerPrefs.GetInt("Width");
        }
        else
        {
            width = 1980;
        }
        if (PlayerPrefs.HasKey("Height"))
        {
            height = PlayerPrefs.GetInt("Height");
        }
        else
        {
            height = 1080;
        }

        if (PlayerPrefs.HasKey("Fullscreen")){
            isFullscreen = PlayerPrefs.GetInt("Fullscreen") != 0;
        } else
        {
            isFullscreen = true;
        }

        PlayerPrefs.SetInt("Width", width);
        PlayerPrefs.SetInt("Height", height);
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        ResolutionLabel.text = width.ToString() + " x " + height.ToString(); 
        

        Screen.fullScreen = isFullscreen;
        if (isFullscreen)
        {
            OnOffText.text = "ON";
        }
        else
        {
            OnOffText.text = "OFF";
        }
    }
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

    public void FullScreen()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        if(isFullscreen)
        {
            OnOffText.text = "ON";
        }
        else
        {
            OnOffText.text = "OFF";
        }
    }

    public void Resolution()
    {
        string text = ResolutionDropdown.options[ResolutionDropdown.value].text;
        int xIndex = text.IndexOf('x');
        width = int.Parse(text.Substring(0, xIndex - 1));
        height = int.Parse(text.Substring(xIndex + 1));
        
        PlayerPrefs.SetInt("Width", width);
        PlayerPrefs.SetInt("Height", height);
        PlayerPrefs.Save();

        Screen.SetResolution(width, height, isFullscreen);
        
    }

    private void ResetPrefs()
    {
        string[] keys = new string[5] { PlayerPrefs.GetString("Interact"), PlayerPrefs.GetString("Jump"),
            PlayerPrefs.GetString("Dash"), PlayerPrefs.GetString("Attack"), PlayerPrefs.GetString("Cast") };
        int fullScreen = PlayerPrefs.GetInt("Fullscreen");
        int screenWidth = PlayerPrefs.GetInt("Width");
        int screenHeight = PlayerPrefs.GetInt("Height");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("FullScreen", fullScreen);
        PlayerPrefs.SetInt("Width", screenWidth);
        PlayerPrefs.SetInt("Height", screenHeight);


        PlayerPrefs.SetString("Interact", keys[0]);
        PlayerPrefs.SetString("Jump", keys[1]);
        PlayerPrefs.SetString("Dash", keys[2]);
        PlayerPrefs.SetString("Attack", keys[3]);
        PlayerPrefs.SetString("Cast", keys[4]);

        PlayerPrefs.SetInt("MaxHealth", 3);
        PlayerPrefs.SetInt("CurrHP", 3);
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("MeleeDmg", 3);
        
        PlayerPrefs.SetFloat("RespawnX", -35f);
        PlayerPrefs.SetFloat("RespawnY", -3.5f);
        PlayerPrefs.SetInt("Scene", 0);

        PlayerPrefs.SetInt("ItemBought1", 0);
        PlayerPrefs.SetInt("ItemBought2", 0);
        PlayerPrefs.SetInt("ItemBought3", 0);
        PlayerPrefs.Save();
    }
}
