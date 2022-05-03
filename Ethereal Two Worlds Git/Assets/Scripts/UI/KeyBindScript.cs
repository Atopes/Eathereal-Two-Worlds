using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class KeyBindScript : MonoBehaviour
{
    public Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public TextMeshProUGUI interact, jump, dash, attack, cast;

    private GameObject currKey;

    private void Start()
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
            keys.Add("Jump", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump")));
        } else
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

        interact.text = keys["Interact"].ToString();
        jump.text = keys["Jump"].ToString();
        dash.text = keys["Dash"].ToString();
        attack.text = keys["Attack"].ToString();
        cast.text = keys["Cast"].ToString();
    }

    private void OnGUI()
    {
        if (currKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                keys[currKey.name] = e.keyCode;
                switch (currKey.name)
                {
                    case "Interact Button":
                        PlayerPrefs.SetString("Interact", e.keyCode.ToString());
                        break;
                    case "Jump Button":
                        PlayerPrefs.SetString("Jump", e.keyCode.ToString());
                        Debug.Log("Ulozeny skok: " + e.keyCode.ToString());
                        break;
                    case "Dash Button":
                        PlayerPrefs.SetString("Dash", e.keyCode.ToString());
                        break;
                    case "Attack Button":
                        PlayerPrefs.SetString("Attack", e.keyCode.ToString());
                        break;
                    case "Cast Button":
                        PlayerPrefs.SetString("Cast", e.keyCode.ToString()); 
                        break;
                }
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
}
