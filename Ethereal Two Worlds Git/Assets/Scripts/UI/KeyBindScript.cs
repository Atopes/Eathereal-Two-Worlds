using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class KeyBindScript : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public TextMeshProUGUI left, right, jump;

    private GameObject currKey;

    private void Start()
    {
        keys.Add("Left", KeyCode.LeftArrow);
        keys.Add("Right", KeyCode.RightArrow);
        keys.Add("Jump", KeyCode.Space);

        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        jump.text = keys["Jump"].ToString();
    }

    private void OnGUI()
    {
        if (currKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keys[currKey.name] = e.keyCode;
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
