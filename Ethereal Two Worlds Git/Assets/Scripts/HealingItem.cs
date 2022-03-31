using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    public BoxCollider2D itemCollision, playerCollision; //Colliders of the Zone and the Player
    public PlayerStatistics playerStatistics; //Reference to the playerStatistics script
    public int healAmount;

    // Update is called once per frame
    void Update()
    {
        if (playerCollision.IsTouching(itemCollision) && PlayerStatistics.currentHP < PlayerStatistics.healthPoints){ //Checking for collision 
            playerStatistics.takeDamage(-healAmount);
            Destroy(gameObject);
        }

    }
}
