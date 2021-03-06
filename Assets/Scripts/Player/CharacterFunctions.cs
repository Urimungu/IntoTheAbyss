﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFunctions : CharacterStats
{

    //Initialization
    private void Start(){
        InitializeInventory();
        CanLook = true;
    }

    //General
    public bool CanMove {
        get => _canMove;
        set => _canMove = value;
    }
    public bool CanLook {
        get => _canLook;
        set {
            _canLook = value;
            if (value){
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    public bool CanInteract {
        get => _canInteract;
        set => _canInteract = value;
    }
    public bool CanOpenInventory {
        get => _canOpenInventory;
        set => _canOpenInventory = value;
    }

    //Dynamic Variables
    public float MovementSpeed
    {
        get => _movementSpeed;
        set => _movementSpeed = value;
    }

    //Interaction
    public float InteractionDistance {
        get => _interactionDistance;
        set => _interactionDistance = value;
    }
    public float InteractionRadius {
        get => _interactionRadius;
        set => _interactionRadius = value;
    }
    public LayerMask InteractionLayerMask {
        get => _interactionLayerMask;
        set => _interactionLayerMask = value;
    }
    public GameObject HoveringOnObject {
        get => _hoveringOnObject;
        set {
            //Makes sure the function doesn't run more than once per hover
            if (_hoveringOnObject == value) return;

            //Sets objects to match
            _hoveringOnObject = value;

            //Starts Hovering
            if (value != null) {
                OnStartHover(value);
                return;
            }

            //Ends Hovering
            OnEndHover();
        }
    }

    //Movement
    public float WalkingSpeed
    {
        get => _walkingSpeed;
        set => _walkingSpeed = value;
    }
    public float RunningSpeed
    {
        get => _runningSpeed;
        set => _runningSpeed = value;
    }

    //Camera Options
    public CameraFunctions CamFunctions {
        get => _camFunctions != null ? _camFunctions : _camFunctions = PlayerCamera.GetComponent<CameraFunctions>();
    }

    //References
    public Rigidbody PlayerRigidbody {
        get => _playerRigidbody = GetComponent<Rigidbody>();
    }
    public GameObject PlayerCamera {
        get => _playerCamera != null ? _playerCamera : _playerCamera = transform.Find("Camera").gameObject;
    }

    //Reference Check
    protected bool GameManagerExists {
        get => GameManager.Manager != null;
    }
    protected bool PlayerUIExists {
        get => GameManagerExists && GameManager.Manager.PlayerUI != null;
    }

    //Inventory
    private void InitializeInventory() {
        for (int i = 0; i < 35; i++) {
            Inventory.Add(new Item());
        }
    }
    protected bool isInventoryOpen;
    public List<Item> Inventory {
        get => _inventory;
        set => _inventory = value;
    }
    public void OpenContainer(ref List<Item> containedItems, string name) {
        //There is no player UI in the scene
        if (!PlayerUIExists) return;

        OpenInventory();
        GameManager.Manager.PlayerUI.OpenStorage(ref containedItems, name);
        CanMove = false;
    }
    protected void OpenInventory() {

        //There is no inventory
        if (!PlayerUIExists) return;

        //Toggles back and forth wether to open the inventory or not
        isInventoryOpen = !isInventoryOpen;
        CanLook = !isInventoryOpen;
        CanInteract = !isInventoryOpen;

        //Opens the inventory and frees the mouse
        if (isInventoryOpen){
            GameManager.Manager.PlayerUI.OpenInventory(Inventory);
            return;
        }

        //Locks the player back into FPS mode
        GameManager.Manager.PlayerUI.HideInventory();
        GameManager.Manager.PlayerUI.HideStorage();
        CanMove = true;
    }

    //Additional Functions
    public GameObject ObjectInteraction() {
        //Initialization
        Ray ray = new Ray(CamFunctions.CameraHolder.transform.position, CamFunctions.CameraHolder.transform.forward);
        bool hitObject = Physics.SphereCast(ray, InteractionRadius, out RaycastHit hit, InteractionDistance, InteractionLayerMask);

        //Exits if nothing was hit
        if (!hitObject) {
            HoveringOnObject = null;
            return null;
        }

        //Makes sure there is an object
        if (hit.collider != null) {
            if (hit.collider.GetComponent<Interactable>() != null) {
                HoveringOnObject = hit.collider.gameObject;
                return hit.collider.gameObject;
            }
        }

        HoveringOnObject = null;
        return null;
    }
    public void PickUpitem(Item item) {
        //Makes sure the player doesn't already have an item of that type
        foreach (Item x in Inventory.FindAll(x => x.ID == item.ID)) {
            if (x.Quantity < x.MaxHoldCount){
                MessageTerminal("<Color=#ff0000>" + item.Name + "</color> was added to stack in inventory.");
                x.Quantity++;
                return;
            }
        }

        //Adds the item to an empty slot and if the player has maxed items, it creates a new spot
        if (Inventory.Find(x => x.ID == 0) != null)
            Inventory[Inventory.FindIndex(x => x.ID == 0)] = item;
        else
            Inventory.Add(item);

        //Sends message to the private terminal
        MessageTerminal("<Color=#ff0000>" + item.Name + "</color> was added to inventory.");

        //Clears the hovering on text
        HoveringOnObject = null;
    }
    public void OnStartHover(GameObject obj) {
        //Exits if the player UI is not set up as a reference
        if (!PlayerUIExists) return;

        //Sets the name in the indicator
        GameManager.Manager.PlayerUI.UpdateCrossHairText(obj.name);

    }
    public void OnEndHover() {
        //Exits if the player UI is not set up as a reference
        if (!PlayerUIExists) return;

        //Hides the indicator
        GameManager.Manager.PlayerUI.HideCrossHairText();
    }
    protected void MessageTerminal(string message) {
        //Exits if there is no game Manager
        if (!GameManagerExists) return;

        GameManager.Manager.TerminalMessage(message);

    }
}
