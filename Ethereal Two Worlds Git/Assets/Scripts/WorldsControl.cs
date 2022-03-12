using System.Collections.Generic;
using UnityEngine;
public class WorldsControl : MonoBehaviour
{
    public GameObject BlueWalls; //Parent object of all "Blue" walls - can be renamed later
    public GameObject RedWalls; //Parent object of all "Red" walls - can be renamed later
    private bool isRedUp = true; // Checks which of the 2 wall types are up
    List<GameObject> RedObjects,BlueObjects; //List of all the coins in the scene
    private int blueCount, redCount; // Counter for the blue and red objects
    private Color blueTransparent,blue,redTransparent,red; // Setting up the correct colors
    private void Start(){
        //Defining the colors in RGBA spectre
        blueTransparent = new Color(0.1f, 0.1f, 0.4f, 0.2f);
        blue = new Color(0.1f, 0.1f, 0.4f, 1f);
        redTransparent = new Color(0.5f, 0.1f, 0.1f, 0.2f);
        red = new Color(0.5f, 0.1f, 0.1f, 1f);
        //Defining the Lists that hold the game objects
        RedObjects = new List<GameObject>();
        BlueObjects = new List<GameObject>();
        //Getting the number of objects that will be in both lists
        redCount = RedWalls.transform.childCount;
        blueCount = BlueWalls.transform.childCount;
        //Adding objects to the Red list
        for(int i = 0; i < redCount; i++){
            RedObjects.Add(RedWalls.transform.GetChild(i).gameObject);
        }
        //Adding objects to the Blue list
        for (int i = 0; i < blueCount; i++){
            BlueObjects.Add(BlueWalls.transform.GetChild(i).gameObject);
        }
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.E) && isRedUp){ //Checking for E input and if red objects are up 
            for(int i = 0; i < blueCount; i++) {
                BlueObjects[i].GetComponent<BoxCollider2D>().enabled=true; //Enables collision with blue walls colliders
                BlueObjects[i].GetComponent<SpriteRenderer>().color = blue; //Sets blue colors transparency to full
            }
            for (int i = 0; i < redCount; i++){
                RedObjects[i].GetComponent<BoxCollider2D>().enabled = false; // Disables collison with red object colliders
                RedObjects[i].GetComponent<SpriteRenderer>().color = redTransparent; // Sets red colors transparency to 10%
            }
            isRedUp = false;
        }
        if (Input.GetKeyDown(KeyCode.R) && !isRedUp){ //Checking for E input and if red objects are down
            for (int i = 0; i < blueCount; i++){
                BlueObjects[i].GetComponent<BoxCollider2D>().enabled =false; // Disables collison with blue object colliders
                BlueObjects[i].GetComponent<SpriteRenderer>().color = blueTransparent; // Sets blue colors transparency to 10 %
            }
            for (int i = 0; i < redCount; i++){
                RedObjects[i].GetComponent<BoxCollider2D>().enabled=true; // Enables collision with red walls colliders
                RedObjects[i].GetComponent<SpriteRenderer>().color = red; // Sets red colors transparency to full
            }
            isRedUp = true;
        }
    }
}