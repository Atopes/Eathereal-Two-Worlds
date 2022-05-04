using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndFlagNext : MonoBehaviour
{
    public PolygonCollider2D FlagCollider;
    public BoxCollider2D PlayerCollider;
    public float nextXposition, nextYposition;
    void Update(){
        if (PlayerCollider.IsTouching(FlagCollider)){ //Looking for collision with the "flag"
            PlayerStatistics.PlayerRespawnPoint = new Vector3(nextXposition,nextYposition,1);
            PlayerPrefs.SetFloat("RespawnX", nextXposition);
            PlayerPrefs.SetFloat("RespawnY", nextYposition);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Loads in next scene , cen be re-done to open some sort of menu
        }
        
    }
}
