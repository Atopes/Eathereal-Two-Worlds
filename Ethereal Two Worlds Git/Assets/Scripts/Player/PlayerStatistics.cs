using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    public static int healthPoints=3,currentHP=3,coins=0,meleeDamage =3; //Different values
    public HealthBar healthBar; // Reference to the Health Bar script
    public static Vector3 PlayerRespawnPoint;
    public bool canTakeDamage = true; // Defines if player can take damage - is invincible
    public Animator animator;
    private void Start(){
        PlayerRespawnPoint = gameObject.transform.position; // Sets the respawn point to the players starting location
    }
    public void takeDamage(int health){ // Method to deal damage to the player
        if (canTakeDamage)
        {
            animator.SetBool("Hurt", true);
            currentHP -= health; // Damages player
            animator.SetInteger("CurrHealth", currentHP);
            healthBar.SetHealth(currentHP); // Setting health bar to the new current hp value 
            canTakeDamage = false; // Makes player invincible
            StartCoroutine(damageTimer()); 
        }
        
    }
    IEnumerator damageTimer()
    {

        yield return new WaitForSecondsRealtime((float)0.01);
        animator.SetBool("Hurt", false);
        yield return new WaitForSecondsRealtime((float)0.99); // Waits 1 s lol
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
