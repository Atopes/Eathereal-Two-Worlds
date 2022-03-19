using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour{
    public int hp; // Hit points of the box - damageableObject
    public void TakeDamage(int damage){
        hp -= damage; // Damages the object
        if(hp <=0){
            Destroy(gameObject); // Destroys the object upon reaching 0 hp
        }
    }
}
