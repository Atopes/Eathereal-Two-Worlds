using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    public BoxCollider2D itemCollision; //Colliders of the Zone and the Player
    private PlayerMovement playerMovement; //Reference to the playerMovement script
    private PlayerStatistics playerStatistics; //Reference to the playerStatistics script
    public int healAmount;
    private AudioSource collectSound;
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerStatistics = FindObjectOfType<PlayerStatistics>();
        collectSound = GameObject.FindGameObjectWithTag("CollectSound").GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (playerMovement.playerColision.IsTouching(itemCollision) && PlayerStatistics.currentHP < PlayerStatistics.healthPoints){ //Checking for collision 
            playerStatistics.healPlayer(healAmount);
            collectSound.Play();
            Destroy(gameObject);
        }

    }
}
