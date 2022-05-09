using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mommon : MonoBehaviour{
    public int healthPoints = 50, attackPower = 1; //Stats of the final boss
    public GameObject barObject,burningHands; // Object of boss health bar and boss basic attack
    public BoxCollider2D enemyCollider, playerColision, barTrigger, wallDetection; // BoxColliders needed for the fight
    public CircleCollider2D playerAttackHitBox; // Attack hit box of the player
    public Rigidbody2D enemyRB; // Boss Rigid body
    public Slider healthBar; // Reference to the slider of boss health bar
    public int movementSpeed =5,maximumVel =4;  ////Stats of the final boss
    public GameObject eye,ringPrefab,ringTip,fireBallPrefab;// Game objects that spawn during the boss fight
    public GameObject torch1, torch2, torch3, torch4; // Reference to the torches in the boss room
    public Animator animator; // Reference to the boss animator
    public Button resetButton; // Button reference
    private PlayerMovement playerMovement; //Reference to the PlayerMovement script
    private int layerWalls; // Reference to the Walls layer
    public static bool isAlive = true,otherWorld = false;
    public static int torchesDown = 0; // Count of torches destroyed
    private bool started = false, canMove = true, isMoveTimerRunning = false, seesPlayer = false,canTakeDamage=true,phase2 =false;
    private Vector3 seekDistance = new Vector3(20f, 0),enemyScale; // See distance of the boss
    private RaycastHit2D hit; // Raycast that check if player is in line of sight
    private Vector2 movement; // Movement variables
    public AudioSource doomSound, soundtrack; // Reference to the music objects

    private bool isPlayerAttacking = false;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>(); // Finds playerMovement script
        layerWalls = LayerMask.NameToLayer("Walls");// Defines the objects on the Walls layer 
        enemyScale = gameObject.transform.localScale; // Reference to the starting local scale of enemy
        healthBar.value = healthPoints; // Set health bar to full
        movement.x = -1; //Which way is the boss moving
        Flip();//Changes the movement direction
    }
    void Update() {
        if (playerColision.IsTouching(barTrigger) && isAlive) { // Starts the boss fight
            started = true;
            resetButton.interactable = false; // No resets in boss room , there is no escape
            barObject.SetActive(true); // Sets health bar visible
            soundtrack.Pause();
            doomSound.Play(); // Starts the boss music
        }if(PlayerStatistics.currentHP == 0){ //What happens if player dies - it resets everything to start value 
            started = false;
            otherWorld = false;
            barObject.SetActive(false);
            healthPoints = 50;
            healthBar.value = healthPoints;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            torch1.SetActive(false);
            torch2.SetActive(false);
            torch3.SetActive(false);
            torch4.SetActive(false);
            torchesDown = 0;
            phase2 = false;
            movementSpeed = 5;
            maximumVel = 4;
            doomSound.Stop();
            soundtrack.UnPause();
        }
        ///////////////////////MOVEMENT//////////////////////////
        if (started && canMove) {
            animator.SetBool("isMoving", true); //Starts move animation
            if (wallDetection.IsTouchingLayers(1 << layerWalls)) { //Checks if the enemy is touching a wall 
                movement.x = -movement.x; // Changes the direction enemy is moving in
                enemyRB.velocity = new Vector2(0, enemyRB.velocity.y); // Nulifies the enemies momentum when changing directions
                Flip(); // Changes the way the enemy is looking 
            }
            if (enemyRB.velocity.x > maximumVel && movement.x > 0) {
                enemyRB.velocity = new Vector2(maximumVel, enemyRB.velocity.y);//Makes sure enemy doesnt become Sonic(enemy velocity stays under the limit)
            }
            else if (enemyRB.velocity.x < -maximumVel && movement.x < 0) {
                enemyRB.velocity = new Vector2(-maximumVel, enemyRB.velocity.y);//Makes sure enemy doesnt become Sonic(enemy velocity stays under the limit)
            }
            enemyRB.AddForce(new Vector2(movement.x * movementSpeed, 0), ForceMode2D.Force); // Moving with enemies rigid body component in a certain direction
        }
        if (started && !isMoveTimerRunning) {
            StartCoroutine(MoveTimer1()); // Makes boss move every 5 seconds
        }
        if(enemyRB.velocity.x == 0){
            animator.SetBool("isMoving", false);// Stops move animation 
        }
        /////////////PLAYER DETECTION/////////////
        //Reycast that makes the boss see u
        hit = Physics2D.Linecast(eye.transform.position, eye.transform.position + (seekDistance * movement.x), 1 << LayerMask.NameToLayer("Player"));
        if (hit.collider != null) {
            seesPlayer = true;
        }else{
            StartCoroutine(ForgetTimer()); // Timer after which boss forgets seeing player
        }
        ////////////BASIC ATTACK/////////
        if (seesPlayer && canMove){ // Starts the attack if is moving and sees player
            animator.SetBool("Attacking", true);
            burningHands.SetActive(true);
        }else{ //Otherwise doesnt attack
            animator.SetBool("Attacking", false);
            burningHands.SetActive(false);
        }
        ///////////HIT DETECTION///////////
        if (Input.GetKeyDown(playerMovement.attackKey)){
            StartCoroutine(HitTimer()); //Better detection of getting hit
        }
        if (isPlayerAttacking){
            if (playerAttackHitBox.IsTouching(enemyCollider) && canTakeDamage){
                TakeDamage(PlayerStatistics.meleeDamage); //Damages boss
            }
        }
    }



    IEnumerator MoveTimer1(){ //Makes boss move every 5 seconds
        isMoveTimerRunning = true;
        if(!seesPlayer){
            movement.x = -movement.x; // Changes the direction enemy is moving in
            Flip(); // Changes the way the enemy is looking 
        }
        yield return new WaitForSecondsRealtime(5);
        canMove = !canMove;
        if (!seesPlayer || phase2){
            //spawns fireball
            Instantiate(fireBallPrefab, new Vector3(gameObject.transform.position.x -3, gameObject.transform.position.y + 5, 1), Quaternion.identity);
        }
        if (phase2){
            //spawns second fireball
            Instantiate(fireBallPrefab, new Vector3(gameObject.transform.position.x + 3, gameObject.transform.position.y + 5, 1), Quaternion.identity);
        }
        isMoveTimerRunning = false;
    }
    IEnumerator ForgetTimer(){ // Makes boss forget he saw player
        yield return new WaitForSecondsRealtime(1);
        if (hit.collider == null){
            seesPlayer = false;
        }
    }
    IEnumerator HitTimer(){ //Enemy can be damaged each 0.5 s
        isPlayerAttacking = true;
        yield return new WaitForSecondsRealtime((float) 0.5);
        isPlayerAttacking = false;
    }
    IEnumerator IFrameAfterHit(){//Enemy can be damaged each 0.5 s
        canTakeDamage = false;
        yield return new WaitForSecondsRealtime((float)0.5);
        canTakeDamage = true;
    }
    IEnumerator WaitForDeath(){ // Waits for death animation
        yield return new WaitForSecondsRealtime((float)0.3);
        barObject.SetActive(false); // Hides health bar
        doomSound.Stop(); // Stops the boss music
        soundtrack.UnPause(); //Unpauses soundtrack
        isAlive = false;
        resetButton.interactable = true; //Makes button work
        //Spawn ring item
        Instantiate(ringPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1), Quaternion.identity);
        ringTip.SetActive(true);// Spawns text about the ring
        Destroy(gameObject);
    }
    private void Flip()
    {
        enemyScale.x = -enemyScale.x; // Flips the local scale
        gameObject.transform.localScale = enemyScale; // Changes the local scale to the new one
    }
    public void TakeDamage(int damage){
        if (!otherWorld){
            healthPoints -= damage; // Damages enemy
            healthBar.value = healthPoints; // Zmeni health bar na aktualne hp
            animator.SetTrigger("hit");
            StartCoroutine(IFrameAfterHit());
        }

        if(healthPoints <= 40 && torchesDown ==0){ //Start the gimmick after each 10hp
            otherWorld = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.4f);
            torch1.SetActive(true);
        }
        if (healthPoints <= 30 && torchesDown == 1)
        {
            otherWorld = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
            torch2.SetActive(true);
        }
        if(healthPoints <= 25){//Enters 2nd phase
            phase2 = true;
            movementSpeed = 8;
            maximumVel = 7;
        }
        if (healthPoints <= 20 && torchesDown == 2)
        {
            otherWorld = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
            torch3.SetActive(true);
        }
        if (healthPoints <= 10 && torchesDown == 3)
        {
            otherWorld = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
            torch4.SetActive(true);
        }

        isPlayerAttacking = false;
        if (healthPoints <= 0){ //Kills boss
            animator.SetBool("isDead",true);
            canMove = false;
            StartCoroutine(WaitForDeath());
        }
    }
}
