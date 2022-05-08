using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartGameAfterCutScene : MonoBehaviour
{
    public GameObject skip;
    private bool isUp=false;

    private KeyCode interactKey;
    [SerializeField]
    private TextMeshProUGUI text;
    void Start()
    {
        interactKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact"));
        text.text = interactKey.ToString(); 
        StartCoroutine(waitForCutscene());
    }
    private void Update(){
        if (Input.GetKeyDown(interactKey) && isUp){
            StopAllCoroutines();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (Input.anyKeyDown && !isUp){
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
