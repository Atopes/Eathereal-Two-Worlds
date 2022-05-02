using System.Collections;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float playerMovementSpeed = 2f, JumpForce = 2f, coyoteTime = 0.2f, coyoteTimeCounter, knockbackForce = 2f;
    public Rigidbody2D PlayerRigidBody; // Player's rigid body
    Vector2 movement; // Variable for declaring movement on X & Y axis
    public BoxCollider2D playerColision; // Player's physicall collider
    public BoxCollider2D groundCheck; // Collider for checking if player is touching ground / platforms
    private DialogueManager dialogueManager;
    public GameObject player; // Reference for player object
    public GameObject bulletPrefab; // Reference to the bullet prefab
    public ParticleSystem DashParticle; // Reference to the dash particle
    private bool isDashing = false, isShooting = false, isGrounded = true, isAttacking = false, isDashFall = false;
    public static bool isFacingRight = true, canDoubleJump = false, canWallJump = false, canShoot = false,canMove=true;
    private int extrajumps = 1, platformsLayer, wallsLayer, layerDamageableObjects, layerEnemies, layerPlayer;
    private Vector3 playerScale; // Local scale of the player used for flipping
    private Collider2D colliderC; // Collider that gets referenced upon attacking - internal
    public Animator animator;
    public CircleCollider2D playerMeleeCollider;//Reference to the players melee attack hitbox
    private KeyCode jumpKey, dashKey, attackKey, castKey;
    
    private void Start()
    {
        if (PlayerPrefs.HasKey("Jump"))
        {
            jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump"));
        } else
        {
            jumpKey = KeyCode.Space;
        }
        if (PlayerPrefs.HasKey("Dash"))
        {
            dashKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Dash"));
        }
        else
        {
            dashKey = KeyCode.LeftShift;
        }
        if (PlayerPrefs.HasKey("Attack"))
        {
            attackKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Attack"));
        }
        else
        {
            attackKey = KeyCode.C;
        }
        if (PlayerPrefs.HasKey("Cast"))
        {
            castKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Cast"));
        }
        else
        {
            castKey = KeyCode.X;
        }

        platformsLayer = LayerMask.NameToLayer("Platforms"); // Defines the objects on the Platforms layer 
        wallsLayer = LayerMask.NameToLayer("Walls"); //Defines the objects on the Walls layer
        layerDamageableObjects = LayerMask.NameToLayer("DamageableObjects"); // Defines the objects on the DamageableObjects layer
        layerEnemies = LayerMask.NameToLayer("Enemies");
        layerPlayer = LayerMask.NameToLayer("Player");
        playerScale = player.transform.localScale; // Defines players starting local scale
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    void Update()
    {
        //Getting X & Y axis input (WASD / arrow keys)
        if (canMove)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else{
            movement = new Vector2(0, 0);
        }
        //Jump mechanics
        if (isDashFall)
        {
            PlayerRigidBody.velocity = new Vector2(PlayerRigidBody.velocity.x, 0);
        }
        if (groundCheck.IsTouchingLayers(1 << platformsLayer) || groundCheck.IsTouchingLayers(1 << layerDamageableObjects))
        { // Checking if the player ground collider is touching anything on the platforms layer
            isGrounded = true;
            animator.SetBool("Grounded", true);
        }
        else
        {
            isGrounded = false;
            animator.SetBool("Grounded", false);
        }
        if (!canDoubleJump)
        { // Jump mechanics if player can't double jump
            if (Input.GetKeyDown(jumpKey) && coyoteTimeCounter > 0)
            {// Checking if space is pressed and if the player is NOT in the air
                PlayerRigidBody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse); // Adding a force that makes the player jump 
            }
        }
        else
        {  // Jump mechanics if player can double jump
            if (Input.GetKeyDown(jumpKey) && extrajumps >= 1)
            { // Checking if space is pressed and player has any extra jumps
                PlayerRigidBody.velocity = new Vector2(movement.x, 0); // Resets the Y axis velocity of player RB 
                PlayerRigidBody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse); // Jumps again
                extrajumps--;
            }
            if (isGrounded)
            {
                extrajumps = 1; // Resseting extra jumps if player touches ground 
                coyoteTimeCounter = coyoteTime; // Resets coyote time counter
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime; // Subtracts time from coyote timer
            }
        }
        if (canWallJump)
        { // Checking if player can wall jump
            if (playerColision.IsTouchingLayers(1 << wallsLayer) && Input.GetKeyDown(jumpKey))
            { // Checking if player is next to a wall
                isGrounded = true; // Making him grounded = resseting his extra jumps
            }
        }
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // Resets the coyote time timer
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // Subtracts current time out of coyote timer
        }
        if (Input.GetKeyUp(jumpKey))
        {
            PlayerRigidBody.velocity = new Vector2(movement.x, (PlayerRigidBody.velocity.y / 2)); // Makes player jump lower upon quick space release
            coyoteTimeCounter = 0; // Sets coyote timer to 0
        }
        //Changes the side the character is facing 
        if (movement.x == 1 && !isFacingRight)
        {
            isFacingRight = true;
            playerScale.x = (float)1.32;
            player.transform.localScale = playerScale;
        }
        else if (movement.x == -1 && isFacingRight)
        {
            isFacingRight = false;
            playerScale.x = (float)-1.32;
            player.transform.localScale = playerScale;
        }
        if (movement.x != 0)
        {
            animator.SetInteger("Mov_Speed", 1);
        }
        else if (movement.x == 0)
        {
            animator.SetInteger("Mov_Speed", 0);
        }
        //Dash mechanics
        if (Input.GetKeyDown(dashKey))
        { // Looking for dash inputs
            if (!isDashing)
            { // Checking if player is not already dashing 
                if (isFacingRight)
                {
                    // player.transform.localScale = new Vector3((float)-1.6, 1, 1); // Transforms the player object to represent the slide
                    PlayerRigidBody.AddForce(new Vector2(15, 0), ForceMode2D.Impulse);
                }
                else
                {
                    // player.transform.localScale = new Vector3((float)1.6, 1, 1);
                    PlayerRigidBody.AddForce(new Vector2(-15, 0), ForceMode2D.Impulse);
                }
                DashParticle.Play(); // Initiates the dash paticle
                PlayerRigidBody.gravityScale = 0; // Makes player not fall during dash
                isDashFall = true;
                isDashing = true;
                StartCoroutine(Dash()); // Starts the dash timer
            }
        }
        // Shooting mechanics
        if (Input.GetKeyDown(castKey) && !isShooting && canShoot)
        { //Checking if X is pressed and player can shoot
            animator.SetTrigger("Cast");
            StartCoroutine(preShootTimer());            
            StartCoroutine(Shoot()); // Starts the shooting timer for when the player can shoot again
        }

        IEnumerator preShootTimer()
        {
            yield return new WaitForSecondsRealtime((float)0.4);
            Instantiate(bulletPrefab, new Vector3(player.gameObject.transform.position.x + ((float)0.5 * playerScale.x), player.gameObject.transform.position.y - (float)1, 1), Quaternion.identity); // Spawns a bullet
            isShooting = true;
        }

        // Melee attack mechanics
        if (Input.GetKeyDown(attackKey) && !isAttacking){ // Checking if C is pressed and if player can attack
            animator.SetTrigger("Attack");
            StartCoroutine(SlashTimer());
            StartCoroutine(Slash());
        }
        if (PlayerRigidBody.velocity.y > 0)
        {
            animator.SetBool("Rising", true);
        }
        else
        {
            animator.SetBool("Rising", false);
        }
        if(dialogueManager.getState()){
            PlayerRigidBody.velocity = new Vector2(0,0);
            movement = new Vector2(0,0);
            canMove = false;
        }
    }
    private void FixedUpdate()
    {
        // Updating player's position
        transform.position += new Vector3(movement.x, 0, 0) * Time.deltaTime * playerMovementSpeed; // Changing players position based on inputs 

    }
    private void ReturnValuesAfterDash()
    { //Returns all the normal player values should the dash end/stop

        PlayerRigidBody.velocity = new Vector2(0, PlayerRigidBody.velocity.y);
        PlayerRigidBody.gravityScale = 4;
    }
    public void KnockBack(Vector3 position)
    {
        float distance = gameObject.transform.position.x - position.x;
        ResetVelocity();
        if (distance >= 0)
        {
            PlayerRigidBody.velocity = new Vector2(0, 0);
            PlayerRigidBody.AddForce(new Vector2(1 * knockbackForce, 12), ForceMode2D.Impulse);
            coyoteTimeCounter -= 1;
            isGrounded = false;
        }
        else
        {
            PlayerRigidBody.velocity = new Vector2(0, 0);
            PlayerRigidBody.AddForce(new Vector2(-1 * knockbackForce, 12), ForceMode2D.Impulse);
            coyoteTimeCounter -= 1;
            isGrounded = false;
        }
    }
    public void ResetVelocity()
    {
        PlayerRigidBody.velocity = new Vector2(0, 0);
    }
    public void FreezePlayer()
    {
        PlayerRigidBody.gravityScale = 5000;
        movement = new Vector2(0, 0);
    }
    public void UnFreezePlayer()
    {
        PlayerRigidBody.gravityScale = 4;
    }
    IEnumerator Dash()
    { // Dash timer
        Physics2D.IgnoreLayerCollision(layerPlayer, layerEnemies, true);
        yield return new WaitForSecondsRealtime((float)0.2);
        Physics2D.IgnoreLayerCollision(layerPlayer, layerEnemies, false);
        isDashFall = false;
        //Sets all values back to normal after 1 second
        DashParticle.Stop();
        ReturnValuesAfterDash();
        yield return new WaitForSecondsRealtime((float)1); //Puts the players dash on 1 s cooldown
        isDashing = false;
    }
    IEnumerator Shoot()
    { // Shooting timer 
        yield return new WaitForSecondsRealtime((float)0.5);
        isShooting = false;
    }
    IEnumerator Slash() {
        yield return new WaitForSecondsRealtime((float)1);
        isAttacking = false;
    }
    IEnumerator SlashTimer(){ 
        yield return new WaitForSecondsRealtime((float)0.2);
        if (playerMeleeCollider.IsTouchingLayers(1 << layerDamageableObjects) || playerMeleeCollider.IsTouchingLayers(1 << layerEnemies))
        { // Checks if player melee attack collider is touching anything on damageAble layer
            playerMeleeCollider.enabled = false;
            if (isFacingRight)
            {
                colliderC = Physics2D.OverlapCircle(new Vector3(playerMeleeCollider.transform.position.x + (float)0.6, playerMeleeCollider.transform.position.y, 1), (float)0.1);
            }
            else
            {
                colliderC = Physics2D.OverlapCircle(new Vector3(playerMeleeCollider.transform.position.x - (float)0.6, playerMeleeCollider.transform.position.y, 1), (float)0.1);
            }
            colliderC.SendMessage("TakeDamage", PlayerStatistics.meleeDamage); // Activaates the TakeDamage method on the object that was hit 
        }
        playerMeleeCollider.enabled = true;
        isAttacking = true;
    }

}