using System;
using UnityEngine;

public class Interactable : MonoBehaviour{

    [Header("Options")]
    [SerializeField] protected bool _canInteract;
    [SerializeField] protected bool _locked;
    protected Interactor _interaction;

    //Delegation
    public delegate void Interactor(CharacterFunctions stats);

    //Options
    protected bool CanInteract {
        get => _canInteract;
        set => _canInteract = value;
    }
    protected bool Locked {
        get => _locked;
        set => _locked = value;
    }
    protected Interactor Interaction {
        get => _interaction;
        set => _interaction = value;
    }

    //Functions
    public void Interact(CharacterFunctions stats) {
        if (!CanInteract) return;

        //Sends message if locked
        if (Locked) {
            MessageTerminal("Cannot to open.");
            return;
        }

        //Interacts if it's not locked and can be interacted with
           Interaction(stats);
    }

    //Sends a message to the terminal
    public void MessageTerminal(string message) {
        if (GameManager.Manager == null) return;

        GameManager.Manager.TerminalMessage(message);
    }
}
