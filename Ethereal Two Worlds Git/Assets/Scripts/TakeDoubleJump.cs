using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDoubleJump : MonoBehaviour
{
    private PlayerMovement playerMovement; //Reference to the playerMovement script
    public BoxCollider2D objectCollider; // Reference to the objects collider
    void Start(){
        playerMovement = FindObjectOfType<PlayerMovement>(); // Defines playerMovement script
    }
    void Update(){
        if (playerMovement.playerColision.IsTouching(objectCollider)){ // Checking for collisio between player collider and object collider
            PlayerMovement.canDoubleJump = false; // Forbids player to double jump
            Destroy(gameObject); // Destroys the game object
        }
    }
}
