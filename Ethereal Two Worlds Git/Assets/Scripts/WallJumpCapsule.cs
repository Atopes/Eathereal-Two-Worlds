using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpCapsule : MonoBehaviour { 
    public PlayerMovement playerMovement;
    public CapsuleCollider2D objectCollider;
    void Update(){
    if (playerMovement.playerColision.IsTouching(objectCollider)){
        PlayerMovement.canWallJump = true;
        Destroy(gameObject);
        }
    }
}
