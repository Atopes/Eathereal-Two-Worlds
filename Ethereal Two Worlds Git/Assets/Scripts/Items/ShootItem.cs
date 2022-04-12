using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootItem : MonoBehaviour
{
    public BoxCollider2D objectCollider;
    private PlayerMovement playerMovement;

    private void Start()
    {
     playerMovement = FindObjectOfType<PlayerMovement>();
    }
    void Update()
    {
        if (playerMovement.playerColision.IsTouching(objectCollider))
        { // Checking for collisio between player collider and object collider
            PlayerMovement.canShoot = true; // Allows player to double jump
            Destroy(gameObject); // Destroys the game object
        }
    }
}
