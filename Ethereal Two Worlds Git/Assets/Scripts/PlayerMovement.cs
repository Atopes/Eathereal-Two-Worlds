using System.Collections;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float playerMovementSpeed = 2f;
    public float JumpForce = 2f;
    public Rigidbody2D PlayerRigidBody; // Player's rigid body
    Vector2 movement; // Variable for declaring movement on X & Y axis
    public BoxCollider2D playerColision; // Player's physicall collider
    public GameObject player;
    private bool isDashing = false;
    void Update(){
        //Getting X & Y axis input (WASD / arrow keys)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //Jump mechanics
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(PlayerRigidBody.velocity.y) < 0.001f){ // Checking if space is pressed and if the player is NOT in the air
            PlayerRigidBody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse); // Adding a force that makes the player jump
        }
        transform.position += new Vector3(movement.x, 0, 0) * Time.deltaTime * playerMovementSpeed; // Updating player's position
        if (Input.GetKeyDown(KeyCode.LeftShift) && movement.x != 0){ // Looking for dash inputs
            if (!isDashing) { // Checking if player is not already dashing 
                player.transform.localScale = new Vector3((float)1.6, 1, 1); // Transforms the player object to represent the slide
                PlayerRigidBody.gravityScale = 6; // Setting gravity scale higher to make player fall a bit faster - shortens jump distance as well
                isDashing = true; 
                StartCoroutine(Dash()); // Starts the dash timer
            }
        }
    }
    IEnumerator Dash() { // Dash timer
        yield return new WaitForSeconds(1f);
        //Sets all values back to normal after 1 second
        player.transform.localScale = new Vector3(1,(float) 1.6, 1);
        PlayerRigidBody.gravityScale = 4;
        isDashing = false;
    }
}