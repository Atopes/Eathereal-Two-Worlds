using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue; // Dialogue object
    public BoxCollider2D triggerArea; // Area in which you can start the dialogue
    private PlayerMovement playerMovement; // Reference to the playerMovement script
    public GameObject prompt; // Prompt that shows up near interactable items
    private bool isPromptUp; // State of the prompt

    private KeyCode interactKey;
    private void Start(){
        playerMovement = FindObjectOfType<PlayerMovement>(); // Defines playerMovement script
        if (PlayerPrefs.HasKey("Interact"))
        {
            interactKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact"));
        }
        else
        {
            interactKey = KeyCode.E;
        }
    }
    public void Update(){
        if (playerMovement.playerColision.IsTouching(triggerArea)){
            if (Input.GetKeyDown(interactKey) && FindObjectOfType<DialogueManager>().getState() == false){
                TriggerDialogue(); // Starts the dialogue if player is in the right area , pressed E and there is no other dialogue on the screen
            }else if (Input.GetKeyDown(interactKey) && FindObjectOfType<DialogueManager>().getState() == true){
                FindObjectOfType<DialogueManager>().DisplayNextSentence(); // Displays the next sentence if the player pressed E and has an active dialogue
            }
            if (!isPromptUp){
                prompt.SetActive(true);
                isPromptUp = true; // Puts the prompt up if player is in the area
            }
        }else{
            if (isPromptUp){
                prompt.SetActive(false);
                isPromptUp = false; // Puts the prompt up if player is NOT in the area
            }
          //  FindObjectOfType<DialogueManager>().EndDialogue();
        }
    }
    public void TriggerDialogue(){
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue); // Starts Dialogue
    }
}
