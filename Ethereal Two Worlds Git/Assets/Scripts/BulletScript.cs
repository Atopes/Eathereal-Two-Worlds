using UnityEngine;

public class BulletScript : MonoBehaviour{
    public CapsuleCollider2D bulletCollider; // Reference to the collider of the bullet
    public int bulletSpeed; // Speed at which the bullet travels
    private float xMovement; // Direction the bullet travels in
    private int layerPlatforms,layerWalls; // References to the layers that affect the bullets
    private void Start(){
        layerPlatforms = LayerMask.NameToLayer("Platforms"); // Defines the objects on the Platforms layer 
        layerWalls = LayerMask.NameToLayer("Walls");// Defines the objects on the Walls layer 
        if (PlayerMovement.isFacingRight){ // Checks which direction is player standing in and send the bullet in the according direction
            xMovement = 1;
        }
        else{
            xMovement = -1;
        }
    }
    void Update() { 
        if (bulletCollider.IsTouchingLayers(1 << layerPlatforms) || bulletCollider.IsTouchingLayers(1 << layerWalls)){ //Checking for the collision with Walls,Platforms Layers
            Destroy(gameObject); //Destroying bullet on contact
        }
        transform.position += new Vector3(xMovement, 0, 0) * Time.deltaTime * bulletSpeed; //Changing the bullets position in the world space
    }
}
