using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour{

    [Header("Public References")]
    public static GameManager Manager;
    public PlayerUIFunctions PlayerUI;

    [Header("Protected References")]
    [SerializeField] protected Text _terminalText;

    //Protected References
    protected Text TerminalText {
        get => _terminalText != null ? _terminalText : _terminalText = transform.Find("Text").GetComponent<Text>();
    }
    protected List<string> TerminalMessages = new List<string>();

    //Functions
    private void Awake(){
        //Creates a game manager Singleton
        if (Manager == null){
            Manager = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    public void TerminalMessage(string message) {
        TerminalMessages.Add(message);
        UpdateTerminal();
        StartCoroutine(RemoveMessage(message));
    }
    protected void UpdateTerminal() {
        string temp = "";

        //Displays all of the messages
        for (int i = 0; i < TerminalMessages.Count; i++) {
            temp += "\n" + TerminalMessages[i];
        }

        //Displays the end result message
        TerminalText.text = temp;
    }
    IEnumerator RemoveMessage(string text) {
        yield return new WaitForSeconds(5);
        TerminalMessages.Remove(TerminalMessages[0]);
        UpdateTerminal();
    }
}
