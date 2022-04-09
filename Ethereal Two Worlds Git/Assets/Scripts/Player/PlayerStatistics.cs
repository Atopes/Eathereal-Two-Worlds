using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    public static int healthPoints=3,currentHP=3,coins=0,meleeDamage =3; //Different values
    public HealthBar healthBar; // Reference to the Health Bar script
    public static Vector3 PlayerRespawnPoint; // Reference to the players respawn point world location
    public bool canTakeDamage = true; // Defines if player can take damage - is invincible
    private void Start(){
        PlayerRespawnPoint = gameObject.transform.position; // Sets the respawn point to the players starting location
    }
    public void takeDamage(int health){ // Method to deal damage to the player
        if (canTakeDamage){
            currentHP -= health; // Damages player
            if(currentHP > healthPoints){
                currentHP = healthPoints;
            }
            healthBar.SetHealth(currentHP); // Setting health bar to the new current hp value 
            canTakeDamage = false; // Makes player invincible
            StartCoroutine(damageTimer()); 
        }
        
    }
    IEnumerator damageTimer(){
        yield return new WaitForSecondsRealtime((float) 0.5); // Waits 0.5 s lol
        canTakeDamage = true; // Makes player mortal 
    }
}
