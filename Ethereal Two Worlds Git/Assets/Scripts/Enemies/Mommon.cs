using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mommon : MonoBehaviour{
    public int healthPoints = 50, attackPower = 1;
    public GameObject enemyCollider,playerColision;
    public Rigidbody2D enemyRB;
    private bool started=false,isFacingRight=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
        
    }
}
