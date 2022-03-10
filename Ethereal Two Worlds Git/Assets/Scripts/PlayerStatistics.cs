using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    public static int healthPoints=3,currentHP=3,coins=0; //Different values
    public HealthBar healthBar; // Reference to the Health Bar script
    public void takeDamage(int health){ // Method to deal damage to the player
        currentHP -= health; 
        healthBar.SetHealth(currentHP); // Setting health bar to the new current hp value 
    }
}
