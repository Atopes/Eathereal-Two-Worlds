using UnityEngine;
public class JumpCapsule : MonoBehaviour{
    public PlayerMovement playerMovement; //Reference to the playerMovement script
    public CapsuleCollider2D objectCollider; // Reference to the objects collider
    void Update(){
        if (playerMovement.playerColision.IsTouching(objectCollider)) { // Checking for collisio between player collider and object collider
            PlayerMovement.canDoubleJump = true; // Allows player to double jump
            Destroy(gameObject); // Destroys the game object
        }
    }
}
