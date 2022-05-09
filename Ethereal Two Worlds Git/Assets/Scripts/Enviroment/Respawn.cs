using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {
    public ParticleSystem particles; // Reference to the paricles game object
    public GameObject player; // Reference to the player object
    public HealthBar healthBar; // Reference to the healthBar script
    public BoxCollider2D respawnPointCollider, PlayerCollider; // Essential colliders
    private PlayerStatistics playerStatistics;
    private Vector3 RespawnPointLocation; // World position of the players respawn point
    private Vector3 offSet;
    private KeyCode interactKey;
    private void Start() {
        offSet = new Vector3(0, 1, 0);
        RespawnPointLocation = gameObject.transform.position + offSet; // Defines the location of the Respawn point
        interactKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact"));
        playerStatistics = FindObjectOfType<PlayerStatistics>(); 
    }
    private void Update() {
        if (respawnPointCollider.IsTouching(PlayerCollider) && Input.GetKeyDown(interactKey)) { // Checks if player is touching the respawn point
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
        PlayerMovement.canMove=true;
        StartCoroutine(healBugFix()); // Sets players hp to maximum
        particles.Play(); // Starts the particle effect
    }
    private void SetHpToMax(){ // Sets players hp to maximum
        PlayerStatistics.currentHP = PlayerStatistics.healthPoints;
        healthBar.healthText.text = PlayerStatistics.currentHP + "/" + PlayerStatistics.healthPoints;
        healthBar.slider.value = PlayerStatistics.currentHP;
    }
    IEnumerator healBugFix(){
        yield return new WaitForSecondsRealtime((float)0.25); // Waits 0.3 s lol
        playerStatistics.healPlayer(0);
        SetHpToMax();
    }
    public void SliderFix()
    {
        playerStatistics.healPlayer(0);
        SetHpToMax();
    }
}
