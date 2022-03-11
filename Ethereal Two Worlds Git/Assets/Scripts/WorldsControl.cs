using System.Collections.Generic;
using UnityEngine;
public class WorldsControl : MonoBehaviour
{
    public GameObject BlueWalls; //Parent object of all "Blue" walls - can be renamed later
    public GameObject RedWalls; //Parent object of all "Red" walls - can be renamed later
    private bool isRedUp = true;
    List<GameObject> RedObjects,BlueObjects; //List of all the coins in the scene
    private int blueCount, redCount;
    private Color blueTransparent,blue,redTransparent,red;
    private void Start()
    {
        blueTransparent = new Color(0.1f, 0.1f, 0.4f, 0.2f);
        blue = new Color(0.1f, 0.1f, 0.4f, 1f);
        redTransparent = new Color(0.5f, 0.1f, 0.1f, 0.2f);
        red = new Color(0.5f, 0.1f, 0.1f, 1f);
        RedObjects = new List<GameObject>();
        BlueObjects = new List<GameObject>();
        redCount = RedWalls.transform.childCount;
        blueCount = BlueWalls.transform.childCount;
        for(int i = 0; i < redCount; i++){
            RedObjects.Add(RedWalls.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < blueCount; i++)
        {
            BlueObjects.Add(BlueWalls.transform.GetChild(i).gameObject);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isRedUp)
        {
            for(int i = 0; i < blueCount; i++)
            {
                BlueObjects[i].GetComponent<BoxCollider2D>().enabled=true;
                BlueObjects[i].GetComponent<SpriteRenderer>().color = blue;
            }
            for (int i = 0; i < redCount; i++)
            {
                RedObjects[i].GetComponent<BoxCollider2D>().enabled = false;
                RedObjects[i].GetComponent<SpriteRenderer>().color = redTransparent;
            }
            isRedUp = false;
        }
        if (Input.GetKeyDown(KeyCode.R) && !isRedUp)
        {
            for (int i = 0; i < blueCount; i++)
            {
                BlueObjects[i].GetComponent<BoxCollider2D>().enabled =false;
                BlueObjects[i].GetComponent<SpriteRenderer>().color = blueTransparent;
            }
            for (int i = 0; i < redCount; i++)
            {
                RedObjects[i].GetComponent<BoxCollider2D>().enabled=true;
                RedObjects[i].GetComponent<SpriteRenderer>().color = red;
            }
            isRedUp = true;
        }
    }
}