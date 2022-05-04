using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameAfterCutScene : MonoBehaviour
{
    public GameObject skip;
    private bool isUp=false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitForCutscene());
    }
    private void Update(){
        if (Input.GetKeyDown(KeyCode.Escape) && isUp){
            StopAllCoroutines();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (Input.GetKeyDown(KeyCode.Escape)&& !isUp){
            skip.SetActive(true);
            isUp = true;
        }
    }
    IEnumerator waitForCutscene()
    {
        yield return new WaitForSecondsRealtime(16);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
