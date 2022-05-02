using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HomeCutScene : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public BoxCollider2D triggerArea,playerCollider;
    public GameObject prompt;
    private bool isPrompUp=false,wasCutscenePlayed=false;
    void Update(){
        if(playerCollider.IsTouching(triggerArea) && wasCutscenePlayed==false){
            if (!isPrompUp){
                prompt.SetActive(true);
                isPrompUp = true;    
            }
            if (Input.GetKeyDown(KeyCode.E)){
                playableDirector.Play();
                wasCutscenePlayed = true;
                PlayerMovement.canMove = false;
                
                StartCoroutine(waitForCutscene());
            }
        }
        else{
            if (isPrompUp){
                prompt.SetActive(false);
                isPrompUp = false;
            }
        }
    }
    IEnumerator waitForCutscene(){
        yield return new WaitForSeconds((float)playableDirector.duration);
        PlayerMovement.canMove = true;
    }
}
