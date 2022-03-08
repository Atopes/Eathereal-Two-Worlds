using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    public GameObject CollectiblesParentObject;
    public CircleCollider2D playerColision;
    List<GameObject> coins;
    private CircleCollider2D coinCollision;
    private int pocetCollectiblov;
    void Start()
    {
        coins = new List<GameObject>();
        pocetCollectiblov = CollectiblesParentObject.transform.childCount;
        for (int i = 0; i < pocetCollectiblov; i++)
        {
            coins.Add(CollectiblesParentObject.transform.GetChild(i).gameObject);
            Debug.Log(coins[i].name);
        }
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < pocetCollectiblov; i++)
        {

            coinCollision = coins[i].GetComponent<CircleCollider2D>();
            if (playerColision.IsTouching(coinCollision))
            {
                Destroy(coins[i]);
                coins.Remove(coins[i]);
                pocetCollectiblov--;
            }
        }
    }
}
