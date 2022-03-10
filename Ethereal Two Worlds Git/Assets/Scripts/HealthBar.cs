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
    private void Start(){
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
            Debug.Log("YOU DIED");
        }
        healthText.text =PlayerStatistics.currentHP+"/"+ PlayerStatistics.healthPoints; //Setting the text displayed in the hp bar
    }
}
