using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public GameObject spawnItemPrefab;
    public BoxCollider2D chestCollider,playerCollider;
    public GameObject prompt;
    public Animator animator;
    private bool isUsed = false, closed = true,promptUp=false;
    private Vector3 offset = new Vector3(0,2f,0);
    private KeyCode interactKey;
    private void Start()
    {
        interactKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact"));
    }
    void Update(){
        if (playerCollider.IsTouching(chestCollider)){
            if (Input.GetKeyDown(interactKey)) {
                if (closed) {
                    if (!isUsed) {
                        Instantiate(spawnItemPrefab, new Vector3(gameObject.transform.position.x + offset.x,gameObject.transform.position.y + offset.y,1), Quaternion.identity);
                        isUsed = true;
                    }
                    closed = false;
                    animator.SetBool("closed", false);
                }else{
                    closed = true;
                    animator.SetBool("closed", true);
                }
            }if (!promptUp){
                prompt.SetActive(true);
                promptUp = true;
            }
        }else{
            if (promptUp){
                prompt.SetActive(false);
                promptUp = false;
            }
        }

    }
}
