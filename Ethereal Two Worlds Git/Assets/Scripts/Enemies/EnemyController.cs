using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour{
    public int healthPoints = 2,attackPower = 1; // Enemy Statistics
    public BoxCollider2D playerCollision,enemyCollision,wallDetection,platformDetection; // Colliders attached to the enemy
    public PlayerStatistics playerStatistics; //Reference to the PlayerStatistics script so our enemy can deal damage
    public Rigidbody2D enemyRB; // Enemies rigid body for smooth movement
    private int layerWalls,layerPlatforms,layerDestroyable; // Reference to the different layers
    public float movementSpeed = 2f,maximumVel = 2f; // Movement speed of the enemy
    Vector3 enemyScale; // Vector that changes the way the enemy is looking - used for changing local scale
    Vector2 movement; // Vector used to define which way is the enemy moving
    void Start() {
        layerWalls = LayerMask.NameToLayer("Walls");// Defines the objects on the Walls layer 
        layerPlatforms = LayerMask.NameToLayer("Platforms"); //Defines the objects on the Platform layer
        layerDestroyable = LayerMask.NameToLayer("DamageableObjects"); // Defines the objects on the DamageableObjects layer
        enemyScale = gameObject.transform.localScale; // Reference to the starting local scale of enemy
        movement.x = 1; // Way the character starts moving once spawned
    }
    // Update is called once per frame
    void Update(){
        if (enemyCollision.IsTouching(playerCollision)){ //Checking for collision with player
            playerStatistics.gameObject.SendMessage("takeDamage",attackPower); //Deals damage to the player upon collision
        }
        //Movement 
        if(enemyRB.velocity.x > maximumVel && movement.x > 0){
            enemyRB.velocity = new Vector2(maximumVel,enemyRB.velocity.y);
        }else if(enemyRB.velocity.x < -maximumVel && movement.x < 0){
            enemyRB.velocity = new Vector2(-maximumVel, enemyRB.velocity.y);
        }
        if (wallDetection.IsTouchingLayers(1 << layerWalls)|| !platformDetection.IsTouchingLayers(1 << layerPlatforms) || wallDetection.IsTouchingLayers(1 << layerDestroyable)){ //Checks if the enemy is touching a wall,damageableObject or platform
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
            Destroy(gameObject); // Destroys enemy when out of health
        }
    }

}
