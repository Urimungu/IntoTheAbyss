using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFunctions : CharacterStats
{

    //Initialization
    private void Start(){
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

    //Dynamic Varaibles
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

    //Inventory
    public List<Item> Inventory {
        get => _inventory;
        set => _inventory = value;
    }

    //Additional Functions
    public GameObject ObjectInteraction() {
        //Initialization
        Ray ray = new Ray(CamFunctions.CameraHolder.transform.position, CamFunctions.CameraHolder.transform.forward);
        bool hitObject = Physics.SphereCast(ray, InteractionRadius, out RaycastHit hit, InteractionDistance, InteractionLayerMask);

        //Exits if nothing was hit
        if (!hitObject) return null;

        //Makes sure there is an object
        if (hit.collider != null) {
            if (hit.collider.GetComponent<Interactable>() != null) {
                return hit.collider.gameObject;
            }
        }

        return null;
    }
    public void PickUpitem(Item item) {
        Inventory.Add(item);
    }
}
