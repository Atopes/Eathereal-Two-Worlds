using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class KeyBindScript : MonoBehaviour
{
    public Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public TextMeshProUGUI /*left, right,*/ jump, dash, attack, cast;

    private GameObject currKey;

    private void Start()
    {
        /*keys.Add("Left", KeyCode.LeftArrow);
        keys.Add("Right", KeyCode.RightArrow);*/
        keys.Add("Jump", KeyCode.Space);
        keys.Add("Dash", KeyCode.LeftShift);
        keys.Add("Attack", KeyCode.C);
        keys.Add("Cast", KeyCode.X); 

        /*left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();*/
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
