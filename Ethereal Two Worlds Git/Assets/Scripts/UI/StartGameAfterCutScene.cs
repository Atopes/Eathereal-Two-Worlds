using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameAfterCutScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitForCutscene());
    }
    IEnumerator waitForCutscene()
    {
        yield return new WaitForSecondsRealtime(16);
        SceneManager.LoadScene("TestArea");
    }
}
