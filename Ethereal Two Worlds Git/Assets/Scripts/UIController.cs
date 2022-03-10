using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    void Start()
    {
        coinsText.text = PlayerStatistics.coins + "";
    }
    void Update()
    {
        
    }
    public void RestartLevel()
    {
        PlayerStatistics.currentHP = PlayerStatistics.healthPoints;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Reloads current Level
    }
    public void SetCoins(int addedAmount)
    {
        PlayerStatistics.coins += addedAmount;
        coinsText.text = PlayerStatistics.coins + "";
    }
}
