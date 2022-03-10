using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndFlag : MonoBehaviour
{
    public PolygonCollider2D FlagCollider;
    public BoxCollider2D PlayerCollider;
    void Update(){
        if (PlayerCollider.IsTouching(FlagCollider)){ //Looking for collision with the "flag"
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Loads in next scene , cen be re-done to open some sort of menu
        }
        
    }
}
