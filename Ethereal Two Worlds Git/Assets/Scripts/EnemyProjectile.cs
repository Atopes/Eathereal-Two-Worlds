using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public CircleCollider2D bulletCollider; // Reference to the collider of the bullet
    public int bulletSpeed; // Speed at which the bullet travels
    private int bulletDamage = 1;
    private float xMovement; // Direction the bullet travels in
    private int layerPlatforms, layerWalls, layerDamageable, layerEnemies; // References to the layers that affect the bullets
    private Vector3 seekDistance = new Vector3(2f,0) ;
    Vector3 bulletScale;
    // Start is called before the first frame update
    void Start()
    {
        layerPlatforms = LayerMask.NameToLayer("Platforms"); // Defines the objects on the Platforms layer 
        layerWalls = LayerMask.NameToLayer("Walls");// Defines the objects on the Walls layer 
        layerEnemies = LayerMask.NameToLayer("Enemies");
        bulletScale = gameObject.transform.localScale;

        RaycastHit2D enemyOnLeft = Physics2D.Linecast(gameObject.transform.position, gameObject.transform.position + seekDistance, 1 << LayerMask.NameToLayer("Enemies"));
        RaycastHit2D enemyOnRight = Physics2D.Linecast(gameObject.transform.position, gameObject.transform.position - seekDistance, 1 << LayerMask.NameToLayer("Enemies"));

        if (enemyOnLeft.collider != null) {
            xMovement = -1;
            Flip();
        }else if(enemyOnRight.collider != null) {
            xMovement = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletCollider.IsTouchingLayers(1 << layerPlatforms) || bulletCollider.IsTouchingLayers(1 << layerWalls)){ //Checking for the collision with Walls,Platforms Layers
            Destroy(gameObject); //Destroying bullet on contact
        }
        if (bulletCollider.IsTouchingLayers(1 << LayerMask.NameToLayer("Player"))){ // Checks if bullet hits something damageable - boxes for example
            Collider2D detectedCollider = Physics2D.OverlapCircle(bulletCollider.transform.position, (float)0.1); // Gets the reference to the damageable object
            if (detectedCollider.gameObject.tag != "IgnoreDamage")
            {
                detectedCollider.gameObject.SendMessage("takeDamage", bulletDamage); // Activates "TakeDamage" method on the found objects
                Destroy(gameObject); // Destroys bullet on contact
            }
        }
        transform.position += new Vector3(xMovement, 0, 0) * Time.deltaTime * bulletSpeed; //Changing the bullets position in the world space
    }
    private void Flip()
    {
        bulletScale.x = xMovement;
        gameObject.transform.localScale = bulletScale;
    }
}
