using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterFunctions{

    private void Update(){
        //Player Movement
        if (CanMove) MovePlayer();
        if (CanLook) CameraMovement();
        if (CanInteract) Interaction();

        //Buttons Presses
        if (CanOpenInventory && Input.GetKeyDown(KeyCode.Tab)) {
            OpenInventory();
        }
    }

    //Moves the player
    private void MovePlayer() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Movement.MovePlayer(PlayerRigidbody, horizontal, vertical, MovementSpeed);
    }

    //Moves the camera
    private void CameraMovement() {
        float horizontal = Input.GetAxisRaw("Mouse X");
        float vertical = Input.GetAxisRaw("Mouse Y");

        //Rotates the player left and right
        if (Mathf.Abs(horizontal) > 0.1f)
            transform.Rotate(transform.up * CamFunctions.CurrentHorSens * horizontal);

        //References the camera to move if the player is moving
        if(Mathf.Abs(vertical) > 0.1f)
            CamFunctions.MoveCamera(vertical);
    }

    //Lets the player interact with things in the outside world
    private void Interaction() {
        var obj = ObjectInteraction();

        //There wasn't anything to interact with
        if (obj == null) return;

        //Interacts with the object
        if (Input.GetKeyDown(KeyCode.E)) {
            obj.GetComponent<Interactable>().Interact(this);
        }
    }
}
