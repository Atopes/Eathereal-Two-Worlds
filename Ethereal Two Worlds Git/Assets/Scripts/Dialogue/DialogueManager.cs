using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText,dialogueText; // Reference for the dialogue window name and text that gives information
    public GameObject DialogueBoxes; // Whole DialogueBoxes GameObject 
    public bool isDialogueUp=false; // Monitors if Dialogue window is up 
    private Queue<string> sentences; // All the sentences that are going to play
    void Start(){
        sentences = new Queue<string>(); // Defines the sentences Queue
    }
    public void StartDialogue(Dialogue dialogue){ // Starts the dialogue , wow
        DialogueBoxes.SetActive(true); // Turns the dialogueBoxes window active
        isDialogueUp = true; // Changes the state
        nameText.text = dialogue.name; // Sets the dialogue window name
        sentences.Clear(); // Clears all text in dialogue window except the name
        foreach(string sentence in dialogue.sentences){
            sentences.Enqueue(sentence); // Q's the sentences
        }
        DisplayNextSentence(); // Shows the next sentence , yes
    }
    public void DisplayNextSentence(){
        if (sentences.Count == 0){
            EndDialogue(); // Ends the dialogue if there are no more sentences to play
            return;
        }
        string sentence = sentences.Dequeue(); // Takes the displayed sentence out of the Q
        StopAllCoroutines(); // Stops the text typing animation
        StartCoroutine(TypeSentence(sentence)); // Start typing the current sentece
    }
    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = ""; // Clears the text area
        foreach(char letter in sentence.ToCharArray()){
            dialogueText.text += letter; // Types each letter 1 by 1 to the text area
            yield return null;
        }
    }
    public void EndDialogue(){
        DialogueBoxes.SetActive(false); // Ends the Dialogue ,damn
        isDialogueUp = false;
        PlayerMovement.canMove = true;
    }
    public bool getState(){  // Returns the state of the dialogue window
        return isDialogueUp;
    }
}
