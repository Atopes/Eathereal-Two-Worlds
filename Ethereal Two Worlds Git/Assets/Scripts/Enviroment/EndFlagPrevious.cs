using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndFlagPrevious : MonoBehaviour
{
    public BoxCollider2D FlagCollider;
    public BoxCollider2D PlayerCollider;
    public float prevXposition, prevYposition;
    void Update(){
        if (PlayerCollider.IsTouching(FlagCollider)){ //Looking for collision with the "flag"
            PlayerMovement.isFacingRight = true;
            PlayerStatistics.PlayerRespawnPoint = new Vector3(prevXposition, prevYposition, 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); //Loads in next scene , cen be re-done to open some sort of menu
        }

    }
}
