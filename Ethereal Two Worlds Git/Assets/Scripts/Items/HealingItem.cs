using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    public BoxCollider2D itemCollision; //Colliders of the Zone and the Player
    private PlayerMovement playerMovement; //Reference to the playerMovement script
    private PlayerStatistics playerStatistics; //Reference to the playerStatistics script
    public int healAmount;
    private AudioSource collectSound;// Reference to the sound
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();//Finds the script
        playerStatistics = FindObjectOfType<PlayerStatistics>();//Finds the script
        collectSound = GameObject.FindGameObjectWithTag("CollectSound").GetComponent<AudioSource>();//Finds the sound
    }
  
    void Update()
    {
        if (playerMovement.playerColision.IsTouching(itemCollision) && PlayerStatistics.currentHP < PlayerStatistics.healthPoints){ //Checking for collision 
            playerStatistics.healPlayer(healAmount); //Heals player by amount
            collectSound.Play();// Plays the collect sound
            Destroy(gameObject);//Destroying the object in the scene
        }

    }
}
