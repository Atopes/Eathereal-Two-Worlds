using System.Collections;
using UnityEngine;
public class PlayerMovement : MonoBehaviour{
    public float playerMovementSpeed = 2f, JumpForce = 2f, reboundForce = 2f;
    public Rigidbody2D PlayerRigidBody; // Player's rigid body
    Vector2 movement; // Variable for declaring movement on X & Y axis
    public BoxCollider2D playerColision; // Player's physicall collider
    public BoxCollider2D groundCheck; // Collider for checking if player is touching ground / platforms
    public GameObject player; // Reference for player object
    public GameObject bulletPrefab; // Reference to the bullet prefab
    private bool isDashing = false,isShooting=false,isGrounded=true; 
    public static bool isFacingRight = true, canDoubleJump = false,canWallJump = false;
    private int extrajumps = 1,platformsLayer,wallsLayer;
    private void Start(){
        platformsLayer = LayerMask.NameToLayer("Platforms"); // Defines the objects on the Platforms layer 
        wallsLayer = LayerMask.NameToLayer("Walls"); //Defines the objects on the Walls layer
    }
    void Update(){
        //Getting X & Y axis input (WASD / arrow keys)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //Jump mechanics
        if (groundCheck.IsTouchingLayers(1 << platformsLayer)) { // Checking if the player ground collider is touching anything on the platforms layer
            isGrounded = true;
        }else{
            isGrounded = false;
        }
        if (!canDoubleJump){ // Jump mechanics if player can't double jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {// Checking if space is pressed and if the player is NOT in the air
                PlayerRigidBody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse); // Adding a force that makes the player jump 
            }
        }else{  // Jump mechanics if player can double jump
            if (Input.GetKeyDown(KeyCode.Space) && extrajumps >= 1) { // Checking if space is pressed and player has any extra jumps
                PlayerRigidBody.velocity = new Vector2(movement.x , 0); // Resets the Y axis velocity of player RB 
                PlayerRigidBody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse); // Jumps again
                extrajumps--; 
            }
            if (canWallJump){ // Checking if player can wall jump
                if (playerColision.IsTouchingLayers(1 << wallsLayer) && Input.GetKeyDown(KeyCode.Space)){ // Checking if player is next to a wall
                    isGrounded = true; // Making him grounded = resseting his extra jumps
                }
            }
            if (isGrounded){
                extrajumps = 1; // Resseting extra jumps if player touches ground 
            }
        }
        // Updating player's position
        transform.position += new Vector3(movement.x, 0, 0) * Time.deltaTime * playerMovementSpeed; // Changing players position based on inputs 
        //Changes the side the character is facing 
        if(movement.x == 1){
            isFacingRight = true;
        }else if (movement.x == -1){
            isFacingRight = false;
        }
        //Dash mechanics
        if (Input.GetKeyDown(KeyCode.LeftShift) && movement.x != 0){ // Looking for dash inputs
            if (!isDashing) { // Checking if player is not already dashing 
                player.transform.localScale = new Vector3((float)1.6, 1, 1); // Transforms the player object to represent the slide
                PlayerRigidBody.gravityScale = 6; // Setting gravity scale higher to make player fall a bit faster - shortens jump distance as well
                playerMovementSpeed = 18;
                isDashing = true; 
                StartCoroutine(Dash()); // Starts the dash timer
            }
        }
        if (movement.x == 0 && isDashing){ //Checking if player is moving during the dash
            // Stopping the players dash should he not move during it
            StopCoroutine(Dash()); 
            ReturnValuesAfterDash();
        }
        // Shooting mechanics
        if (Input.GetKeyDown(KeyCode.X) && !isShooting){ //Checking if X is pressed and player can shoot
            Instantiate(bulletPrefab, new Vector3(player.gameObject.transform.position.x, player.gameObject.transform.position.y, 1), Quaternion.identity); // Spawns a bullet
            isShooting = true;
            StartCoroutine(Shoot()); // Starts the shooting timer for when the player can shoot again
        }
    }
    private void ReturnValuesAfterDash() { //Returns all the normal player values should the dash end/stop
        player.transform.localScale = new Vector3(1, (float)1.6, 1);
        PlayerRigidBody.gravityScale = 4;
        playerMovementSpeed = 10;
        isDashing = false;
    }
    IEnumerator Dash() { // Dash timer
        yield return new WaitForSecondsRealtime((float) 0.5);
        //Sets all values back to normal after 1 second
        ReturnValuesAfterDash();
    }
    IEnumerator Shoot(){ // Shooting timer 
        yield return new WaitForSecondsRealtime((float) 0.5);
        isShooting = false;
    }
}