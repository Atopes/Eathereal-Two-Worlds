using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardZone : MonoBehaviour
{
    public BoxCollider2D zoneCollision, playerCollision; //Colliders of the Zone and the Player
    public PlayerStatistics playerStatistics; //Reference to the playerStatistics script
    private bool isIn = false;
    public bool hasKnockBack = true;
    public int damage = 1;
    private AudioSource hitSound; //Reference to the sound
    private void Start()
    {
        hitSound = GameObject.FindGameObjectWithTag("HitSound").GetComponent<AudioSource>(); // Find the sound
    }
    void Update(){
        if (playerCollision.IsTouching(zoneCollision)) { //Checking for collision 
            if (!isIn) {
                hitSound.Play(); // Plays the sound
                isIn = true; // Setting player state in the hazard zone
                if (hasKnockBack) {
                    FindObjectOfType<PlayerMovement>().KnockBack(gameObject.transform.position);
                }
                StartCoroutine(DamageOverTIme()); // Starting Coroutine for taking dmg over time
            }
        }else {
        if (isIn){
                isIn = false; // Setting player state out of hazard zone
                StopCoroutine(DamageOverTIme()); // Stoping Coroutine for taking dmg over time
            }
        }
    }
    IEnumerator DamageOverTIme() {//Damage over time Courutine , while in hazard zone deals 1 dmg each second
        while (isIn) {
            playerStatistics.takeDamage(damage);
            yield return new WaitForSeconds(1f);
        }
    }
}
