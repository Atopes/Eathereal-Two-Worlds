using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour{
    public GameObject self;
    public Rigidbody2D bulletRigidBody;
    public CapsuleCollider2D bulletCollider;
    public int bulletSpeed;
    private float xMovement;
    private int layerPlatforms,layerWalls;
    private void Start(){
        xMovement = Input.GetAxisRaw("Horizontal");
        layerPlatforms = LayerMask.NameToLayer("Platforms");
        layerWalls = LayerMask.NameToLayer("Walls");
        if (PlayerMovement.isFacingRight){
            xMovement = 1;
        }
        else{
            xMovement = -1;
        }
    }
    void Update(){

        if (bulletCollider.IsTouchingLayers(1 << layerPlatforms) || bulletCollider.IsTouchingLayers(1 << layerWalls))
        {
            Destroy(self);
        }
        transform.position += new Vector3(xMovement, 0, 0) * Time.deltaTime * bulletSpeed;
    }
}
