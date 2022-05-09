using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    private BoxCollider2D playerColision; //Player physical collider
    public CircleCollider2D coinCollision; //Coin trigger collider variable
    public PlayerStatistics playerStatistics; //Reference to the playerStatistics script
    public int moneyAmount=1;
    private AudioSource collectSound; // Reference to the sound
    
    public void Start()
    {
        playerColision = GameObject.Find("Player").GetComponent<BoxCollider2D>(); //Finds the collider
        collectSound = GameObject.FindGameObjectWithTag("CollectSound").GetComponent<AudioSource>();//Finds the sound
    }
    void FixedUpdate(){
            if (playerColision.IsTouching(coinCollision)){ //Actually checking for the collision
            FindObjectOfType<UIController>().SetCoins(moneyAmount); //Changes the amount of money player has
            collectSound.Play(); // Plays the collect sound
            Destroy(gameObject); //Destroying the object in the scene
            }
        }
    }
