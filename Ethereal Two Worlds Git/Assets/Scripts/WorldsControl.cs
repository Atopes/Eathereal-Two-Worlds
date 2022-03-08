using UnityEngine;
public class WorldsControl : MonoBehaviour
{
    public GameObject BlueWalls; //Parent object of all "Blue" walls - can be renamed later
    public GameObject RedWalls; //Parent object of all "Red" walls - can be renamed later
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            BlueWalls.SetActive(!BlueWalls.activeSelf); //Changing the active state of all blue walls
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RedWalls.SetActive(!RedWalls.activeSelf); //Changing the active state of all red walls
        }
    }
}