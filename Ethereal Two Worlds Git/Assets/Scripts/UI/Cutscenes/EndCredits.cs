using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class EndCredits : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public BoxCollider2D triggerArea, playerCollider;
    public GameObject prompt;
    private bool isPrompUp = false;
    public AudioSource soundtrack, piano;

    private KeyCode interactKey;

    private void Start()
    {
        interactKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact"));
    }

    void Update()
    {
        if (playerCollider.IsTouching(triggerArea))
        {
            if (!isPrompUp)
            {
                prompt.SetActive(true);
                isPrompUp = true;
            }
            if (Input.GetKeyDown(interactKey))
            {
                soundtrack.Stop();
                piano.Play();
                playableDirector.Play();
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
    IEnumerator waitForCutscene()
    {
        yield return new WaitForSeconds((float)playableDirector.duration);
        SceneManager.LoadScene("MainMenu");
    }
}
