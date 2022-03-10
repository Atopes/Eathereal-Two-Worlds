using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI healthText;
    private void Start()
    {
        setMaxHealth();
        setMaxHealthText();
    }
    public void setMaxHealth()
    {
        slider.maxValue = PlayerStatistics.healthPoints;
    }
    public void setMaxHealthText()
    {
        healthText.text = PlayerStatistics.currentHP + "/" + PlayerStatistics.healthPoints;
    }
    public void SetHealth(int health)
    {
        slider.value = health;
        if (PlayerStatistics.currentHP <= 0)
        {
            PlayerStatistics.currentHP = 0;
            Debug.Log("YOU DIED");
        }
        healthText.text =PlayerStatistics.currentHP+"/"+ PlayerStatistics.healthPoints;
    }
}
