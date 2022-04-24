using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : MonoBehaviour
{
    public int healthPoints = 2, attackPower = 1; // Enemy Statistics
    public BoxCollider2D playerCollision, enemyCollision, wallDetection, platformDetection; // Colliders attached to the enemy
    public PlayerStatistics playerStatistics; //Reference to the PlayerStatistics script so our enemy can deal damage
    public Rigidbody2D enemyRB; // Enemies rigid body for smooth movement
    public GameObject eye; // Enemies eye
    public GameObject enemeyProjectilePrefab; // Bullet prefab
    private int layerWalls, layerPlatforms, layerDestroyable; // Reference to the different layers
    public float movementSpeed = 2f, maximumVel = 2f; // Movement speed of the enemy
    Vector3 enemyScale; // Vector that changes the way the enemy is looking - used for changing local scale
    Vector2 movement; // Vector used to define which way is the enemy moving
    private Vector3 seekDistance = new Vector3(8f, 0); // See distance
    private bool canShoot=true;
    void Start()
    {
        layerWalls = LayerMask.NameToLayer("Walls");// Defines the objects on the Walls layer 
        layerPlatforms = LayerMask.NameToLayer("Platforms"); //Defines the objects on the Platform layer
        layerDestroyable = LayerMask.NameToLayer("DamageableObjects"); // Defines the objects on the DamageableObjects layer
        enemyScale = gameObject.transform.localScale; // Reference to the starting local scale of enemy
        movement.x = 1; // Way the character starts moving once spawned
    }
    void Update()
    {
        if (enemyCollision.IsTouching(playerCollision)){ //Checking for collision with player
            playerStatistics.gameObject.SendMessage("takeDamage", attackPower); //Deals damage to the player upon collision
        }
        if (wallDetection.IsTouchingLayers(1 << layerWalls) || !platformDetection.IsTouchingLayers(1 << layerPlatforms) || wallDetection.IsTouchingLayers(1 << layerDestroyable)){ //Checks if the enemy is touching a wall,damageableObject or platform
            if (enemyRB.velocity.y > -0.1){ // Should the enemy be falling it wont start spinning xd
                movement.x = -movement.x; // Changes the direction enemy is moving in
                enemyRB.velocity = new Vector2(0, enemyRB.velocity.y); // Nulifies the enemies momentum when changing directions
                Flip(); // Changes the way the enemy is looking 
            }
        }
        RaycastHit2D hit = Physics2D.Linecast(eye.transform.position, eye.transform.position + seekDistance, 1 << LayerMask.NameToLayer("Player")); // This are the enemies eyes
        if (hit.collider != null){ // Checks if  the player is in the FOV
            maximumVel = 0; // Stops the enemy so it can shoot
            if (canShoot){
                Instantiate(enemeyProjectilePrefab, new Vector3(eye.gameObject.transform.position.x + (float)0.6, eye.gameObject.transform.position.y - (float)0.2, 1), Quaternion.identity);
                canShoot = false;
                StartCoroutine(shootTimer()); // Shoots lmao 
            }
        }
        else if (hit.collider == null && maximumVel == 0){
            maximumVel = 2; // Sets the maximum vel up so it can move after shooting
        }
        if (enemyRB.velocity.x < maximumVel && movement.x > 0 || (enemyRB.velocity.x > -maximumVel && movement.x < 0)){
            enemyRB.AddForce(new Vector2(movement.x * movementSpeed, 0), ForceMode2D.Force); // Moving with enemies rigid body component in a certain direction
        }
    }
    private void Flip(){
        enemyScale.x = -enemyScale.x; // Flips the local scale
        gameObject.transform.localScale = enemyScale; // Changes the local scale to the new one
        seekDistance = new Vector3(-seekDistance.x, 0); // Changes the way the enemy is looking
    }
    public void TakeDamage(int damage){
        healthPoints -= damage; // Damages enemy
        if (healthPoints <= 0){
            Destroy(gameObject); // Destroys enemy when out of health
        }
    }
    IEnumerator shootTimer(){
      yield return new  WaitForSecondsRealtime((float) 2); // Defines how often can the enemy shoot
        canShoot = true;
    }
}
