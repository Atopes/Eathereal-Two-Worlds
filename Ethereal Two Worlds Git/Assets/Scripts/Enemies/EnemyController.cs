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
    private Vector3 seekDistance = new Vector3(8f, 0); //line of sight
    private Vector3 biteDistance = new Vector3(1f, 0); //line of sight
    public GameObject eye;
    private bool seenPlayer=false, canAttack = true,canDoDamage=true;
    public Animator animator;
    public GameObject coinPrefab;
    void Start() {
        layerWalls = LayerMask.NameToLayer("Walls");// Defines the objects on the Walls layer 
        layerPlatforms = LayerMask.NameToLayer("Platforms"); //Defines the objects on the Platform layer
        layerDestroyable = LayerMask.NameToLayer("DamageableObjects"); // Defines the objects on the DamageableObjects layer
        layerEnemies = LayerMask.NameToLayer("Enemies");
        enemyScale = gameObject.transform.localScale; // Reference to the starting local scale of enemy
        movement.x = 1; // Way the character starts moving once spawned
        Physics2D.IgnoreLayerCollision(layerEnemies, layerEnemies, true);
    }
    // Update is called once per frame
    void Update(){
        RaycastHit2D left = Physics2D.Linecast(eye.transform.position, eye.transform.position - ((float)1.65*seekDistance * movement.x), 1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D right = Physics2D.Linecast(eye.transform.position, eye.transform.position + ((float)1.5 * seekDistance * movement.x), 1 << LayerMask.NameToLayer("Player"));
        if (left.collider != null || right.collider != null)
        {
            movementSpeed = 5;
            animator.SetBool("Walking",true);
        }
        if (enemyCollision.IsTouching(playerCollision) && canDoDamage){ //Checking for collision with player
            playerStatistics.gameObject.SendMessage("takeDamage",attackPower); //Deals damage to the player upon collision
        }
        RaycastHit2D hit = Physics2D.Linecast(eye.transform.position, eye.transform.position + (seekDistance*movement.x), 1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D bite = Physics2D.Linecast(eye.transform.position, eye.transform.position + (biteDistance * movement.x), 1 << LayerMask.NameToLayer("Player"));
        if (hit.collider != null){
            maximumVel = 4;
            seenPlayer = true;
            animator.SetBool("Running", true);
            if (bite.collider != null && canAttack)
            {
                Attack();
            }
        }
        if (hit.collider == null){
            if(maximumVel == 4){
                maximumVel = 2;
                animator.SetBool("Running", false);
            }
            if (seenPlayer){
                StartCoroutine(Seek());
            }

        }
            //Movement 
        if (enemyRB.velocity.x > maximumVel && movement.x > 0){
            enemyRB.velocity = new Vector2(maximumVel,enemyRB.velocity.y);
        }else if(enemyRB.velocity.x < -maximumVel && movement.x < 0){
            enemyRB.velocity = new Vector2(-maximumVel, enemyRB.velocity.y);
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
            if (Random.Range(1,101) <= 40){
                Instantiate(coinPrefab, new Vector3(gameObject.transform.position.x , gameObject.transform.position.y, 1), Quaternion.identity);
            }
            animator.SetTrigger("Die");
            StartCoroutine(dieTimer());
        }
    }
    IEnumerator Seek()
    {
        yield return new WaitForSecondsRealtime((float)1);
        if(movement.x == 1){
            if(playerCollision.transform.position.x < enemyCollision.transform.position.x){
                movement.x = -1;
                Flip();
            }
        }else if (movement.x==-1){
            if(playerCollision.transform.position.x > enemyCollision.transform.position.x)
            {
                movement.x = 1;
                Flip();
            }
        }
        seenPlayer = false;
    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSecondsRealtime((float)0.25);
        animator.SetTrigger("Attack");
        maximumVel = 4;
    }
    IEnumerator dieTimer()
    {
        maximumVel = 0;
        canAttack = false;
        canDoDamage = false;
        yield return new WaitForSecondsRealtime((float)0.3);
        Destroy(gameObject); // Destroys enemy when out of health
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSecondsRealtime((float)1.25);
        canAttack = true;
    }

    private void Attack()
    {   
            canAttack = false;
            maximumVel = 16;
            enemyRB.velocity = new Vector2(enemyRB.velocity.x *(float) 1.4, enemyRB.velocity.y);
            animator.SetTrigger("Attack");
            StartCoroutine(AttackTimer());
            StartCoroutine(AttackCooldown());   
    }

}
