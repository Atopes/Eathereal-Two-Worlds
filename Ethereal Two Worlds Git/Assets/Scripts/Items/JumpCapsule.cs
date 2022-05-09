using UnityEngine;
public class JumpCapsule : MonoBehaviour{
    private PlayerMovement playerMovement; //Reference to the playerMovement script
    public CapsuleCollider2D objectCollider; // Reference to the objects collider
    private AudioSource collectSound;
    private void Start() {
        playerMovement = FindObjectOfType<PlayerMovement>(); // Defines playerMovement script
        collectSound = GameObject.FindGameObjectWithTag("CollectSound").GetComponent<AudioSource>();
    }
    void Update(){
        if (playerMovement.playerColision.IsTouching(objectCollider)) { // Checking for collisio between player collider and object collider
            PlayerMovement.canDoubleJump = true; // Allows player to double jump
            collectSound.Play();
            Destroy(gameObject); // Destroys the game object
        }
    }
}
