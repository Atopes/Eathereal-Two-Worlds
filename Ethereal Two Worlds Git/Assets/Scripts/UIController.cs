using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    public Respawn respawn;
    public TextMeshProUGUI coinsText; // Reference to the text displayed next to the coin symbol
    void Start() {
        coinsText.text = PlayerStatistics.coins + ""; // Setting the correct number of player's coins on reload and start
    }
    public void RestartLevel(){
        respawn.RespawnPlayer();
    }
    public void SetCoins(int addedAmount){
        PlayerStatistics.coins += addedAmount; // Adds coins to the player
        coinsText.text = PlayerStatistics.coins + ""; // Updates the number of coins displayed in UI
    }
}
