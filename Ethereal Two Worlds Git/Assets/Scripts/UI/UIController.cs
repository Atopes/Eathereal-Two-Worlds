using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class UIController : MonoBehaviour
{
    public Respawn respawn; // Reference to the respawn script
    public TextMeshProUGUI coinsText; // Reference to the text displayed next to the coin symbol
    public AudioSource soundtrack;
    public static float soundtrackTime = 0;
    void Start() {
        coinsText.text = PlayerStatistics.coins + ""; // Setting the correct number of player's coins on reload and start
        SetSoundtrack();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PlayerMovement.canMove != false){
            StartCoroutine(kuscickopocekaj());
        }
    }
    public void RestartLevel(){
        respawn.RespawnPlayer(); // Respawns player
    }
    public void SetCoins(int addedAmount){
        PlayerStatistics.coins += addedAmount; // Adds coins to the player
        coinsText.text = PlayerStatistics.coins + ""; // Updates the number of coins displayed in UI
    }
    public void SetSoundtrack()
    {
        soundtrack.time = soundtrackTime;
    }

    IEnumerator kuscickopocekaj()
    {
        yield return new WaitForSecondsRealtime((float)0.05);
        SceneManager.LoadScene("MainMenu");
    }
}
