using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {
    public ParticleSystem particles; // Reference to the paricles game object
    public GameObject player; // Reference to the player object
    public HealthBar healthBar; // Reference to the healthBar script
    public BoxCollider2D respawnPointCollider, PlayerCollider; // Essential colliders
    private Vector3 RespawnPointLocation; // World position of the players respawn point
    private void Start() {
        RespawnPointLocation = gameObject.transform.position; // Defines the location of the Respawn point
    }
    private void Update() {
        if (respawnPointCollider.IsTouching(PlayerCollider) && Input.GetKeyDown(KeyCode.O)) { // Checks if player is touching the respawn point
            UpdateRespawnPoint();
            SetHpToMax();
            particles.Play();
        }
    }
    private void UpdateRespawnPoint() {
        PlayerStatistics.PlayerRespawnPoint = RespawnPointLocation;  // Sets the players respawn point to new position
    }
    public void RespawnPlayer() {
        FindObjectOfType<PlayerMovement>().ResetVelocity();
        player.transform.position = PlayerStatistics.PlayerRespawnPoint; // Changes players current location
        SetHpToMax(); // Sets players hp to maximum
        particles.Play(); // Starts the particle effect
    }
    private void SetHpToMax(){ // Sets players hp to maximum
        PlayerStatistics.currentHP = PlayerStatistics.healthPoints;
        healthBar.healthText.text = PlayerStatistics.currentHP + "/" + PlayerStatistics.healthPoints;
        healthBar.slider.value = PlayerStatistics.currentHP;
    }
}
