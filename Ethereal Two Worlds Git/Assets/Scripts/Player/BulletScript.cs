using UnityEngine;

public class BulletScript : MonoBehaviour{
    public CircleCollider2D bulletCollider; // Reference to the collider of the bullet
    public int bulletSpeed; // Speed at which the bullet travels
    private int bulletDamage = 1;
    private float xMovement; // Direction the bullet travels in
    private int layerPlatforms,layerWalls,layerDamageable,layerEnemies; // References to the layers that affect the bullets
    Vector3 bulletScale;
    private void Start(){
        layerPlatforms = LayerMask.NameToLayer("Platforms"); // Defines the objects on the Platforms layer 
        layerWalls = LayerMask.NameToLayer("Walls");// Defines the objects on the Walls layer 
        layerDamageable = LayerMask.NameToLayer("DamageableObjects");
        layerEnemies = LayerMask.NameToLayer("Enemies");
        bulletScale = gameObject.transform.localScale;
        if (PlayerMovement.isFacingRight){ // Checks which direction is player standing in and send the bullet in the according direction
            xMovement = 1;
        }
        else{
            xMovement = -1;
            Flip();
        }
    }
    void Update() {
        Physics2D.IgnoreLayerCollision(0, 0);
        if (bulletCollider.IsTouchingLayers(1 << layerPlatforms) || bulletCollider.IsTouchingLayers(1 << layerWalls)){ //Checking for the collision with Walls,Platforms Layers
            Destroy(gameObject); //Destroying bullet on contact
        }
        if (bulletCollider.IsTouchingLayers(1 << layerDamageable) || bulletCollider.IsTouchingLayers(1 << layerEnemies)) { // Checks if bullet hits something damageable - boxes for example
            Collider2D detectedCollider = Physics2D.OverlapCircle(bulletCollider.transform.position,(float) 0.1); // Gets the reference to the damageable object
            if (detectedCollider.gameObject.tag != "IgnoreDamage"){
                detectedCollider.gameObject.SendMessage("TakeDamage", bulletDamage); // Activates "TakeDamage" method on the found objectsd
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
