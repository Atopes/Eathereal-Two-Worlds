using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    public static int healthPoints=3,currentHP=3,coins=0;
    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void takeDamage(int health)
    {
        currentHP -= health;
        healthBar.SetHealth(currentHP);
    }
}
