using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HomeCutScene : MonoBehaviour
{
    public PlayableDirector playableDirector; //Reference to the cutscene director
    public BoxCollider2D triggerArea,playerCollider; //Reference to the colliders
    public GameObject prompt; //Prompt collider
    private bool isPrompUp=false; //Prompt state
    private static bool wasCutscenePlayed = false;
    public AudioSource soundTrack, cutsceneMusic; //Music references
    private KeyCode interactKey;//Interact key reference

    private void Start(){
        //Loads the interact key
        interactKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact"));    
    }

    void Update(){
        if(playerCollider.IsTouching(triggerArea) && wasCutscenePlayed==false){
            if (!isPrompUp){
                prompt.SetActive(true);//Sets prompt visible
                isPrompUp = true;    
            }
            if (Input.GetKeyDown(interactKey)){
                soundTrack.Pause();//Pause soundtrack
                cutsceneMusic.Play();//Plays cutscene Music
                playableDirector.Play(); // Plays cutscene
                wasCutscenePlayed = true;
                PlayerMovement.canMove = false; //Forbids movement
                StartCoroutine(waitForCutscene());
            }
        }
        else{
            if (isPrompUp){
                prompt.SetActive(false); //Sets prompt invisible
                isPrompUp = false;
            }
        }
    }
    IEnumerator waitForCutscene(){ //Wait till cutscene ends
        yield return new WaitForSeconds((float)playableDirector.duration);
        PlayerMovement.canMove = true; //Allows movement
        cutsceneMusic.Stop(); //Stops cutscene music
        soundTrack.UnPause(); // Plays soundtrack
    }
}
