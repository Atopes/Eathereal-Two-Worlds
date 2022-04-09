using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWorldItem : MonoBehaviour
{
    private PlayerMovement playerMovement; //Reference to the playerMovement script
    public BoxCollider2D objectCollider; // Reference to the objects collider

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }
    void Update()
    {
        if (playerMovement.playerColision.IsTouching(objectCollider))
        { // Checking for collisio between player collider and object collider
            WorldsControl.canSwitch = true;
            Destroy(gameObject); // Destroys the game object
        }
    }
}
