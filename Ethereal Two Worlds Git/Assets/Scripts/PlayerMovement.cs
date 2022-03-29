using System.Collections;
using UnityEngine;
public class PlayerMovement : MonoBehaviour{
    public float playerMovementSpeed = 2f, JumpForce = 2f, coyoteTime = 0.2f ,coyoteTimeCounter;
    public Rigidbody2D PlayerRigidBody; // Player's rigid body
    Vector2 movement; // Variable for declaring movement on X & Y axis
    public BoxCollider2D playerColision; // Player's physicall collider
    public BoxCollider2D groundCheck; // Collider for checking if player is touching ground / platforms
    public CircleCollider2D playerMeleeCollider;
    public SpriteRenderer slashSprite;
    public GameObject player; // Reference for player object
    public GameObject bulletPrefab; // Reference to the bullet prefab
    public ParticleSystem DashParticle;
    private bool isDashing = false,isShooting=false,isGrounded=true,isAttacking = false; 
    public static bool isFacingRight = true, canDoubleJump = false,canWallJump = false;
    private int extrajumps = 1,platformsLayer,wallsLayer,layerDamageableObjects;
    private Vector3 playerScale;
    private Collider2D colliderC;

    public Animator animator;
    private void Start(){
        platformsLayer = LayerMask.NameToLayer("Platforms"); // Defines the objects on the Platforms layer 
        wallsLayer = LayerMask.NameToLayer("Walls"); //Defines the objects on the Walls layer
        layerDamageableObjects = LayerMask.NameToLayer("DamageableObjects");
        playerScale = player.transform.localScale;
    }
    void Update(){
        //Getting X & Y axis input (WASD / arrow keys)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //Jump mechanics
        if (groundCheck.IsTouchingLayers(1 << platformsLayer) || groundCheck.IsTouchingLayers(1 << layerDamageableObjects)) { // Checking if the player ground collider is touching anything on the platforms layer
            isGrounded = true;
            animator.SetBool("Grounded", true);
        }
        else{
            isGrounded = false;
            animator.SetBool("Grounded", false);
        }
        if (!canDoubleJump){ // Jump mechanics if player can't double jump
            if (Input.GetKeyDown(KeyCode.Space) && coyoteTimeCounter > 0) {// Checking if space is pressed and if the player is NOT in the air
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
                coyoteTimeCounter = coyoteTime;
            }else{
                coyoteTimeCounter -= Time.deltaTime;
            }
        }
        if (isGrounded){ 
            coyoteTimeCounter = coyoteTime;
        }else{
            coyoteTimeCounter -= Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space)){
            PlayerRigidBody.velocity = new Vector2(movement.x,(PlayerRigidBody.velocity.y/2));
            coyoteTimeCounter = 0;
        }
        // Updating player's position
        transform.position += new Vector3(movement.x, 0, 0) * Time.deltaTime * playerMovementSpeed; // Changing players position based on inputs 
        //Changes the side the character is facing 
        if(movement.x == 1 && !isFacingRight){
            isFacingRight = true;
            playerScale.x = 1;
            player.transform.localScale = playerScale;

        }else if (movement.x == -1 && isFacingRight){
            isFacingRight = false;
            playerScale.x = -1;
            player.transform.localScale = playerScale;
        }
        if (movement.x != 0)
            animator.SetFloat("Mov_Speed", 1);
        else if(movement.x == 0)
            animator.SetFloat("Mov_Speed", 0);
        //Dash mechanics
        if (Input.GetKeyDown(KeyCode.LeftShift)){ // Looking for dash inputs
            if (!isDashing) { // Checking if player is not already dashing 
                if (isFacingRight){
                    // player.transform.localScale = new Vector3((float)-1.6, 1, 1); // Transforms the player object to represent the slide
                    PlayerRigidBody.AddForce(new Vector2(15, 0), ForceMode2D.Impulse);
                }else {
                    // player.transform.localScale = new Vector3((float)1.6, 1, 1);
                    PlayerRigidBody.AddForce(new Vector2(-15, 0), ForceMode2D.Impulse);
                }
                DashParticle.Play();
                PlayerRigidBody.gravityScale = 0;
                isDashing = true; 
                StartCoroutine(Dash()); // Starts the dash timer
            }
        }
        // Shooting mechanics
        if (Input.GetKeyDown(KeyCode.X) && !isShooting) { //Checking if X is pressed and player can shoot
            Instantiate(bulletPrefab, new Vector3(player.gameObject.transform.position.x + (float) 1, player.gameObject.transform.position.y - (float) 0.5, 1), Quaternion.identity); // Spawns a bullet
            isShooting = true;
            StartCoroutine(Shoot()); // Starts the shooting timer for when the player can shoot again
        }
        // Melee attack mechanics
        if(Input.GetKeyDown(KeyCode.C) && !isAttacking){
            slashSprite.enabled = true;
            if (playerMeleeCollider.IsTouchingLayers(1 << layerDamageableObjects)) {
                playerMeleeCollider.enabled = false;
                if (isFacingRight)
                {
                    colliderC = Physics2D.OverlapCircle(new Vector3(playerMeleeCollider.transform.position.x + (float)0.6, playerMeleeCollider.transform.position.y, 1), (float)0.1);
                }else
                {
                    colliderC = Physics2D.OverlapCircle(new Vector3(playerMeleeCollider.transform.position.x - (float)0.6, playerMeleeCollider.transform.position.y, 1), (float)0.1);
                }
                    colliderC.SendMessage("TakeDamage", PlayerStatistics.meleeDamage);
            }
            playerMeleeCollider.enabled = true;
            isAttacking = true;
            StartCoroutine(Slash());
        }

        if (PlayerRigidBody.velocity.y > 0) animator.SetBool("Rising", true);
        else animator.SetBool("Rising", false);
    }
    private void ReturnValuesAfterDash() { //Returns all the normal player values should the dash end/stop
    
        PlayerRigidBody.velocity = new Vector2(0,PlayerRigidBody.velocity.y);
        PlayerRigidBody.gravityScale = 4;
    }
    private void ReturnValuesAfterSlash()
    {
        slashSprite.enabled = false;
    }
    IEnumerator Dash() { // Dash timer
        yield return new WaitForSecondsRealtime((float) 0.2);
        //Sets all values back to normal after 1 second
        DashParticle.Stop();
        ReturnValuesAfterDash();
        yield return new WaitForSecondsRealtime((float) 1); //Puts the players dash on 1 s cooldown
        isDashing = false;
    }
    IEnumerator Shoot(){ // Shooting timer 
        yield return new WaitForSecondsRealtime((float) 0.5);
        isShooting = false;
    }
    IEnumerator Slash()
    { // Shooting timer 
        yield return new WaitForSecondsRealtime((float)0.3);
        ReturnValuesAfterSlash();
        yield return new WaitForSecondsRealtime((float)0.5);
        isAttacking = false;
    }

    

}