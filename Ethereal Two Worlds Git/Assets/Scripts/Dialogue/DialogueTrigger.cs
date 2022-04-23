using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public BoxCollider2D triggerArea;
    public PlayerMovement playerMovement;
    public GameObject prompt;
    private bool isPromptUp;
    public void Update()
    {
        if (playerMovement.playerColision.IsTouching(triggerArea)){
            if (Input.GetKeyDown(KeyCode.E) && FindObjectOfType<DialogueManager>().getState() == false){
                TriggerDialogue();
            }else if (Input.GetKeyDown(KeyCode.E) && FindObjectOfType<DialogueManager>().getState() == true)
            {
                FindObjectOfType<DialogueManager>().DisplayNextSentence();
            }
            if (!isPromptUp){
                prompt.SetActive(true);
                isPromptUp = true;
            }
        }else{
            if (isPromptUp){
                prompt.SetActive(false);
                isPromptUp = false;
            }
          //  FindObjectOfType<DialogueManager>().EndDialogue();
        }
    }
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
