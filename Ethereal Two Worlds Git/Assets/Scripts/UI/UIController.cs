using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    public Respawn respawn; // Reference to the respawn script
    public TextMeshProUGUI coinsText; // Reference to the text displayed next to the coin symbol
    void Start() {
        coinsText.text = PlayerStatistics.coins + ""; // Setting the correct number of player's coins on reload and start
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }
    public void RestartLevel(){
        respawn.RespawnPlayer(); // Respawns player
    }
    public void SetCoins(int addedAmount){
        PlayerStatistics.coins += addedAmount; // Adds coins to the player
        PlayerPrefs.SetInt("Coins", PlayerStatistics.coins);
        PlayerPrefs.Save();
        coinsText.text = PlayerStatistics.coins + ""; // Updates the number of coins displayed in UI
    }
}
