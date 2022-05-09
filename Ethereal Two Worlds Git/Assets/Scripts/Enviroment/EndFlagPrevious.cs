using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndFlagPrevious : MonoBehaviour
{
    public BoxCollider2D FlagCollider;
    public BoxCollider2D PlayerCollider;
    public float prevXposition, prevYposition;
    public AudioSource soundtrack;
    void Update(){
        if (PlayerCollider.IsTouching(FlagCollider)){ //Looking for collision with the "flag"
            PlayerMovement.isFacingRight = false;
            PlayerStatistics.PlayerRespawnPoint = new Vector3(prevXposition, prevYposition, 1);
            PlayerPrefs.SetFloat("RespawnX", prevXposition);
            PlayerPrefs.SetFloat("RespawnY", prevYposition);
            PlayerPrefs.SetInt("Coins", PlayerStatistics.coins);
            PlayerPrefs.SetInt("CurrHP", PlayerStatistics.currentHP);
            UIController.soundtrackTime = soundtrack.time;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2); //Loads in next scene , cen be re-done to open some sort of menu
        }

    }
}
