using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour{
    public int healthPoints = 2,attackPower = 1; // Enemy Statistics
    public BoxCollider2D playerCollision,enemyCollision,wallDetection,platformDetection; // Colliders attached to the enemy
    public PlayerStatistics playerStatistics; //Reference to the PlayerStatistics script so our enemy can deal damage
    public Rigidbody2D enemyRB; // Enemies rigid body for smooth movement
    private int layerWalls,layerPlatforms,layerDestroyable,layerEnemies; // Reference to the different layers
    public float movementSpeed = 0,maximumVel = 2f; // Movement speed of the enemy
    Vector3 enemyScale; // Vector that changes the way the enemy is looking - used for changing local scale
    Vector2 movement; // Vector used to define which way is the enemy moving
    private Vector3 seekDistance = new Vector3(12f, 0); //line of sight
    private Vector3 biteDistance = new Vector3(1.5f, 0); //line of sight
    public GameObject eye; // Empty game object that defines the start of , line of sight
    private bool seenPlayer=false, canAttack = true,canDoDamage=true;
    public Animator animator; // Reference to the object that handles animations
    public GameObject coinPrefab; // Prefab of the collectible coin
    private AudioSource hitSound; //Sound that plays when enemy hits player
    void Start() {
        hitSound = GameObject.FindGameObjectWithTag("HitSound").GetComponent<AudioSource>();
        layerWalls = LayerMask.NameToLayer("Walls");// Defines the objects on the Walls layer 
        layerPlatforms = LayerMask.NameToLayer("Platforms"); //Defines the objects on the Platform layer
        layerDestroyable = LayerMask.NameToLayer("DamageableObjects"); // Defines the objects on the DamageableObjects layer
        layerEnemies = LayerMask.NameToLayer("Enemies"); // Defines the objects on the Enemies layer
        enemyScale = gameObject.transform.localScale; // Reference to the starting local scale of enemy
        movement.x = 1; // Way the character starts moving once spawned
        Physics2D.IgnoreLayerCollision(layerEnemies, layerEnemies, true); //Makes enemies not collide with each other
    }
    // Update is called once per frame
    void Update(){
        // Raycasts that check if player is on left/right of the object
        RaycastHit2D left = Physics2D.Linecast(eye.transform.position, eye.transform.position - ((float)2*seekDistance * movement.x), 1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D right = Physics2D.Linecast(eye.transform.position, eye.transform.position + ((float)2* seekDistance * movement.x), 1 << LayerMask.NameToLayer("Player"));
        // If reycasts find player enemy starts moving
        if (left.collider != null || right.collider != null)
        {
            movementSpeed = 5;
            animator.SetBool("Walking",true);
            animator.SetBool("Running", false);
        }
        if (enemyCollision.IsTouching(playerCollision) && canDoDamage){ //Checking for collision with player
            hitSound.Play(); // Plays the sound
            playerStatistics.gameObject.SendMessage("takeDamage",attackPower); //Deals damage to the player upon collision
        }
        //Checks if player is in line of sight distance
        RaycastHit2D hit = Physics2D.Linecast(eye.transform.position, eye.transform.position + (seekDistance*movement.x), 1 << LayerMask.NameToLayer("Player"));
        //Checks if player is in bite distance
        RaycastHit2D bite = Physics2D.Linecast(eye.transform.position, eye.transform.position + (biteDistance * movement.x), 1 << LayerMask.NameToLayer("Player"));
        if (hit.collider != null){// Starts running if player is in seek distance
            maximumVel = 4;
            movementSpeed = 8;
            seenPlayer = true;
            animator.SetBool("Running", true);
            animator.SetBool("Walking", false);
            if (bite.collider != null && canAttack) // Attacks if player is in bite distance
            {
                Attack();
            }
        }
        if (hit.collider == null){ // Slows down if it doesnt see player
            if(maximumVel == 4){
                maximumVel = 2;
                movementSpeed = 5;
            }
            if (seenPlayer){ //Starts timer to forget seeing player
                StartCoroutine(Seek());
            }

        }
        //Movement 
        if (enemyRB.velocity.x > maximumVel && movement.x > 0){
            enemyRB.velocity = new Vector2(maximumVel,enemyRB.velocity.y);//Makes sure enemy doesnt become Sonic(enemy velocity stays under the limit)
        }
        else if(enemyRB.velocity.x < -maximumVel && movement.x < 0){
            enemyRB.velocity = new Vector2(-maximumVel, enemyRB.velocity.y); //Makes sure enemy doesnt become Sonic(enemy velocity stays under the limit)
        }
        if (wallDetection.IsTouchingLayers(1 << layerWalls)|| !platformDetection.IsTouchingLayers(1 << layerPlatforms) || wallDetection.IsTouchingLayers(1 << layerDestroyable) || wallDetection.IsTouchingLayers(1 << layerPlatforms)){ //Checks if the enemy is touching a wall,damageableObject or platform
            if (enemyRB.velocity.y > -0.1){ // Should the enemy be falling it wont start spinning xd
                movement.x = -movement.x; // Changes the direction enemy is moving in
                enemyRB.velocity = new Vector2(0, enemyRB.velocity.y); // Nulifies the enemies momentum when changing directions
                Flip(); // Changes the way the enemy is looking 
            }
        }
        enemyRB.AddForce(new Vector2(movement.x * movementSpeed, 0), ForceMode2D.Force); // Moving with enemies rigid body component in a certain direction

    }
    private void Flip(){
        enemyScale.x = -enemyScale.x; // Flips the local scale
        gameObject.transform.localScale = enemyScale; // Changes the local scale to the new one
    }
    public void TakeDamage(int damage){
        healthPoints -= damage; // Damages enemy
        if (healthPoints <= 0) {
            if (Random.Range(1,101) <= 40){ // 40% chance that enemy drops coin upon death
                //Spawns the coin
                Instantiate(coinPrefab, new Vector3(gameObject.transform.position.x , gameObject.transform.position.y, 1), Quaternion.identity);
            }
            animator.SetTrigger("Die"); //Starts death animation
            StartCoroutine(dieTimer()); //Timer to wait for animation to end
        }
    }
    IEnumerator Seek(){ // Waits 1 second after not seeing player then changes direction
        yield return new WaitForSecondsRealtime((float)1);
        if(movement.x == 1){
            if(playerCollision.transform.position.x < enemyCollision.transform.position.x){
                movement.x = -1;
                Flip();
            }
        }else if (movement.x==-1){
            if(playerCollision.transform.position.x > enemyCollision.transform.position.x){
                movement.x = 1;
                Flip();
            }
        }
        seenPlayer = false;
    }

    IEnumerator AttackTimer(){ // Waiting for attack animation
        yield return new WaitForSecondsRealtime((float)0.25);
        animator.SetTrigger("Attack");
        maximumVel = 4;
    }
    IEnumerator dieTimer(){ // Waiting for death animation
        maximumVel = 0;
        canAttack = false;
        canDoDamage = false;
        yield return new WaitForSecondsRealtime((float)0.3);
        Destroy(gameObject); // Destroys enemy when out of health
    }

    IEnumerator AttackCooldown(){ // Makes sure that enemy attack only once every 1.25s
        yield return new WaitForSecondsRealtime((float)1.25);
        canAttack = true;
    }

    private void Attack(){   // Gives enemy speed boost before attack
            canAttack = false;
            maximumVel = 16;
            enemyRB.velocity = new Vector2(enemyRB.velocity.x *(float) 1.4, enemyRB.velocity.y);
            StartCoroutine(AttackTimer());
            StartCoroutine(AttackCooldown());   
    }

}
