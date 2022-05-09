using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider slider; //The slider object itself
    public TextMeshProUGUI healthText; //Text displayed in the health bar
    public Respawn respawn; // Reference to the respawn script
    private PlayerMovement playerMovement; // Reference to the playerMovement script
    private PlayerStatistics playerStatistics; // Reference to the playerStatistics script
    private void Start(){
        playerMovement = FindObjectOfType<PlayerMovement>(); //Finds script
        playerStatistics = FindObjectOfType<PlayerStatistics>();//Finds script
        slider.value = PlayerStatistics.currentHP; //Changes value of health bar
        setMaxHealth();
        setMaxHealthText();
    }
    public void setMaxHealth(){
        slider.maxValue = PlayerStatistics.healthPoints; // Setting slider max value to the max hp of the player
    }
    public void setMaxHealthText(){
        healthText.text = PlayerStatistics.currentHP + "/" + PlayerStatistics.healthPoints; //Setting the text displayed in the hp bar
    }
    public void SetHealth(int health){
        slider.value = health; //Setting the value displayed on the slider to the current health
        if (PlayerStatistics.currentHP <= 0){ // Do something if hp is <= then 0
            PlayerStatistics.currentHP = 0;
            playerStatistics.canTakeDamage = false;
            StartCoroutine(respawnTimer()); //Wait to respawn player
        }
        healthText.text =PlayerStatistics.currentHP+"/"+ PlayerStatistics.healthPoints; //Setting the text displayed in the hp bar
    }
    IEnumerator respawnTimer()
    {
        playerMovement.FreezePlayer();
        playerMovement.ResetVelocity();
        PlayerMovement.canMove = false; // Forbids movement
        yield return new WaitForSecondsRealtime((float)0.9); // Waits 0.3 s lol
        respawn.RespawnPlayer(); // Respawns player
        playerMovement.UnFreezePlayer(); // Allows movement
        playerStatistics.canTakeDamage = true;
    }
}
