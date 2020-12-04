using System;
using UnityEngine;

public class Interactable : MonoBehaviour{

    [Header("Options")]
    [SerializeField] protected bool _canInteract;
    protected Interactor _interaction;

    //Delegation
    public delegate void Interactor(CharacterFunctions stats);

    //Options
    protected bool CanInteract {
        get => _canInteract;
        set => _canInteract = value;
    }
    protected Interactor Interaction {
        get => _interaction;
        set => _interaction = value;
    }

    //Functions
    public void Interact(CharacterFunctions stats) {
        if(CanInteract)
           Interaction(stats);
    }
}
