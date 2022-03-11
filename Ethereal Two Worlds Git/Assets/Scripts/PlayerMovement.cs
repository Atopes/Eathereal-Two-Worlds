using System.Collections;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float playerMovementSpeed = 2f, JumpForce = 2f, reboundForce = 2f;
    public Rigidbody2D PlayerRigidBody; // Player's rigid body
    Vector2 movement; // Variable for declaring movement on X & Y axis
    public BoxCollider2D playerColision; // Player's physicall collider
    public BoxCollider2D groundCheck;
    public GameObject player;
    public GameObject bulletPrefab;
    private bool isDashing = false,isShooting=false,isGrounded=true;
    public static bool isFacingRight = true, canDoubleJump = false,canWallJump = false;
    private int extrajumps = 1,platformsLayer,wallsLayer;
    private void Start(){
        platformsLayer = LayerMask.NameToLayer("Platforms");
        wallsLayer = LayerMask.NameToLayer("Walls");
    }
    void Update(){
        //Getting X & Y axis input (WASD / arrow keys)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //Jump mechanics
        if (groundCheck.IsTouchingLayers(1 << platformsLayer)) {
            isGrounded = true;
        }else{
            isGrounded = false;
        }
        if (!canDoubleJump){
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {// Checking if space is pressed and if the player is NOT in the air
                PlayerRigidBody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse); // Adding a force that makes the player jump 
            }
        }else{
            if (Input.GetKeyDown(KeyCode.Space) && extrajumps >= 1) {
                PlayerRigidBody.velocity = new Vector2(movement.x , 0);
                PlayerRigidBody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
                extrajumps--;
            }
            if (canWallJump){
                if (playerColision.IsTouchingLayers(1 << wallsLayer) && Input.GetKeyDown(KeyCode.Space)){
                    isGrounded = true;
                }
            }
            if (isGrounded)
            {
                extrajumps = 1;
            }
        }

        transform.position += new Vector3(movement.x, 0, 0) * Time.deltaTime * playerMovementSpeed; // Updating player's position
        if(movement.x == 1){
            isFacingRight = true;
        }else if (movement.x == -1){
            isFacingRight = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && movement.x != 0){ // Looking for dash inputs
            if (!isDashing) { // Checking if player is not already dashing 
                player.transform.localScale = new Vector3((float)1.6, 1, 1); // Transforms the player object to represent the slide
                PlayerRigidBody.gravityScale = 6; // Setting gravity scale higher to make player fall a bit faster - shortens jump distance as well
                playerMovementSpeed = 18;
                isDashing = true; 
                StartCoroutine(Dash()); // Starts the dash timer
            }
        }
        if (Input.GetKeyDown(KeyCode.X) && !isShooting){
            Instantiate(bulletPrefab, new Vector3(player.gameObject.transform.position.x, player.gameObject.transform.position.y, 1), Quaternion.identity);
            isShooting = true;
            StartCoroutine(Shoot());
        }
        if(movement.x == 0 && isDashing)
        {
            StopCoroutine(Dash());
            ReturnValuesAfterDash();
        }
    }
    private void ReturnValuesAfterDash()
    {
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
    IEnumerator Shoot(){
        yield return new WaitForSecondsRealtime((float) 0.5);
        isShooting = false;
    }
}