using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class KeyBindScript : MonoBehaviour
{
    public System.Collections.Generic.Dictionary<string, KeyCode> keys = new System.Collections.Generic.Dictionary<string, KeyCode>();

    public TextMeshProUGUI interact, jump, dash, attack, cast;

    private GameObject currKey;

    [SerializeField]
    private GameObject keyDuplicatedPanel;
    [SerializeField]
    private Button interactButton, jumpButton, dashButton, attackButton, castButton;

    private void Start()
    {
        LoadKeys();
        SaveKeys();

        interact.text = keys["Interact"].ToString();
        jump.text = keys["Jump"].ToString();
        dash.text = keys["Dash"].ToString();
        attack.text = keys["Attack"].ToString();
        cast.text = keys["Cast"].ToString();
    }

    private void OnGUI()
    {
        Event x = Event.current;

        if (x.mousePosition.x >= interactButton.transform.position.x - interactButton.GetComponent<RectTransform>().rect.width &&
            x.mousePosition.x <= interactButton.transform.position.x + interactButton.GetComponent<RectTransform>().rect.width &&
            x.mousePosition.y >= interactButton.transform.position.y - interactButton.GetComponent<RectTransform>().rect.height &&
            x.mousePosition.y <= interactButton.transform.position.y + interactButton.GetComponent<RectTransform>().rect.height)

        {
            KeyDuplicated();
        }

        if (currKey != null)
        {
            Event e = Event.current;

            if (e.mousePosition.x > interactButton.transform.position.x - interactButton.GetComponent<RectTransform>().rect.width / 2 &&
                e.mousePosition.x < interactButton.transform.position.x + interactButton.GetComponent<RectTransform>().rect.width / 2 &&
                e.mousePosition.x > interactButton.transform.position.y - interactButton.GetComponent<RectTransform>().rect.height / 2 &&
                e.mousePosition.x < interactButton.transform.position.y + interactButton.GetComponent<RectTransform>().rect.height / 2)
                
            {
                Debug.Log("Pojeb sa");
            }
            if (e.isKey)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                keys[currKey.name] = e.keyCode;
                SaveChanged(currKey.name, e.keyCode);
                PlayerPrefs.Save();
                currKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                currKey = null;
            }
        }
    }

    public void ChangeKey(GameObject clicked)
    {
        currKey = clicked;
    }

    private void SaveChanged(string name, KeyCode key)
    {
        PlayerPrefs.SetString(name.Replace(" Button", ""), key.ToString());
        PlayerPrefs.Save();
    }

    private void SaveKeys()
    {
        PlayerPrefs.SetString("Interact", keys["Interact"].ToString());
        PlayerPrefs.SetString("Jump", keys["Jump"].ToString());
        PlayerPrefs.SetString("Dash", keys["Dash"].ToString());
        PlayerPrefs.SetString("Attack", keys["Attack"].ToString());
        PlayerPrefs.SetString("Cast", keys["Cast"].ToString());
        PlayerPrefs.Save();
    }

    private void LoadKeys()
    {
        if (PlayerPrefs.HasKey("Interact"))
        {
            keys.Add("Interact", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact")));
        }
        else
        {
            keys.Add("Interact", KeyCode.E);
        }
        if (PlayerPrefs.HasKey("Jump"))
        {
            keys.Add("Jump", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump")));
        }
        else
        {
            keys.Add("Jump", KeyCode.Space);
        }
        if (PlayerPrefs.HasKey("Dash"))
        {
            keys.Add("Dash", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Dash")));
        }
        else
        {
            keys.Add("Dash", KeyCode.LeftShift);
        }
        if (PlayerPrefs.HasKey("Attack"))
        {
            keys.Add("Attack", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Attack")));
        }
        else
        {
            keys.Add("Attack", KeyCode.C);
        }
        if (PlayerPrefs.HasKey("Cast"))
        {
            keys.Add("Cast", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Cast")));
        }
        else
        {
            keys.Add("Cast", KeyCode.X);
        }
    }

    private void KeyDuplicated()
    {
        StartCoroutine(KeyDuplicatedTimer());
    }

    IEnumerator KeyDuplicatedTimer()
    {
        keyDuplicatedPanel.SetActive(true);
        yield return new WaitForSecondsRealtime((float)0.75);
        keyDuplicatedPanel.SetActive(false);
    }

}
