using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatistics : MonoBehaviour
{
    public static int healthPoints,currentHP,coins,meleeDamage; //Different values
    public HealthBar healthBar; // Reference to the Health Bar script
    public PlayerMovement playerMovement;
    public static Vector3 PlayerRespawnPoint;
    public bool canTakeDamage = true; // Defines if player can take damage - is invincible
    public Animator animator;
    private void Start(){
        PlayerRespawnPoint = new Vector3(/*PlayerPrefs.GetFloat("RespawnX")*/227f, /*PlayerPrefs.GetFloat("RespawnY")*/-3.27f, 1);
        healthPoints = PlayerPrefs.GetInt("MaxHealth");
        currentHP = PlayerPrefs.GetInt("CurrHP");
        coins = PlayerPrefs.GetInt("Coins");
        meleeDamage = PlayerPrefs.GetInt("MeleeDmg");
        PlayerPrefs.SetInt("Scene", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
    }
    public void takeDamage(int health){ // Method to deal damage to the player
        if (canTakeDamage && currentHP > 0)
        {
            animator.SetTrigger("Hurt");
            currentHP -= health; // Damages player
            animator.SetInteger("CurrHealth", currentHP);
            healthBar.SetHealth(currentHP); // Setting health bar to the new current hp value 
            canTakeDamage = false; // Makes player invincible
            StartCoroutine(damageTimer()); 
        }
        
    }
    IEnumerator damageTimer()
    {
        yield return new WaitForSecondsRealtime((float)0.3); // Waits 0.3 s lol
            canTakeDamage = true; // Makes player mortal  
    }
    
    public void healPlayer(int health)
    {
        currentHP += health;
        if (currentHP > healthPoints)
        {
            currentHP = healthPoints;
        }
        healthBar.SetHealth(currentHP);
    }
}
