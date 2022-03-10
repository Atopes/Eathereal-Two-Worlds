using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    public GameObject CollectiblesParentObject; //Object containing all the collectibles
    public BoxCollider2D playerColision; //Player physical collider
    List<GameObject> coins; //List of all the coins in the scene
    private CircleCollider2D coinCollision; //Coin trigger collider variable
    private int CollectiblesCount; // Count of coins 
    void Start()
    {
        // Just finding and adding all coins into the List for later use
        coins = new List<GameObject>();
        CollectiblesCount = CollectiblesParentObject.transform.childCount;
        for (int i = 0; i < CollectiblesCount; i++)
        {
            coins.Add(CollectiblesParentObject.transform.GetChild(i).gameObject);
        }
    }
    void FixedUpdate()
    {
        //Checking for Player collision with coins
        for (int i = 0; i < CollectiblesCount; i++)
        {
            coinCollision = coins[i].GetComponent<CircleCollider2D>(); // Fetching coin trigger collider , why? Shit doesn't work without it .
            if (playerColision.IsTouching(coinCollision)) //Actually checking for the collision
            {
                Destroy(coins[i]); //Destroying the object in the scene
                coins.Remove(coins[i]); //Removing the coins from the List after being picked up
                CollectiblesCount--; // Reducing the count of collectibles after destroying them
            }
        }
    }
}