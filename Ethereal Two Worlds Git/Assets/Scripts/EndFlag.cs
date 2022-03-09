using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndFlag : MonoBehaviour
{
    public PolygonCollider2D FlagCollider;
    public CircleCollider2D PlayerCollider;
    void Update()
    {
        if (PlayerCollider.IsTouching(FlagCollider))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Loads in next scene , cen be re-done to open some sort of menu
        }
        
    }
}
