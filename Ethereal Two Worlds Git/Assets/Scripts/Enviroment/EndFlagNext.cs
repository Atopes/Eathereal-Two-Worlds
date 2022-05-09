using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndFlagNext : MonoBehaviour
{
    public PolygonCollider2D FlagCollider; // collider of the object
    public BoxCollider2D PlayerCollider; // player collider
    public float nextXposition, nextYposition; // Coordinates that player loads in , in the next scene
    private Respawn respawn; // Reference to the respawn script
    public AudioSource soundtrack; // Soundtrack reference
    private void Start()
    {
        respawn = FindObjectOfType<Respawn>(); //Find the respawn script
    }
    void Update(){
        if (PlayerCollider.IsTouching(FlagCollider)){ //Looking for collision with the object
            //Saves player progress
            PlayerStatistics.PlayerRespawnPoint = new Vector3(nextXposition,nextYposition,1);
            PlayerPrefs.SetFloat("RespawnX", nextXposition);
            PlayerPrefs.SetFloat("RespawnY", nextYposition);
            PlayerPrefs.SetInt("Coins", PlayerStatistics.coins);
            PlayerPrefs.SetInt("CurrHP", PlayerStatistics.currentHP);
            UIController.soundtrackTime = soundtrack.time; // Keeps the soundtrack going between scenes
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Loads in next scene , cen be re-done to open some sort of menu
            respawn.RespawnPlayer(); // respawns the player yaaay
        }
        
    }
}
