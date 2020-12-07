using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable{

    [SerializeField] protected Item self;

    //Initializes the Delegation
    private void Start(){
        Interaction = PickUpItem;
        gameObject.name = self.Name;
    }

    //Picks up the item and adds it into the inventory of the player
    public void PickUpItem(CharacterFunctions stats) {
        stats.PickUpitem(self);
        Destroy(gameObject);
    }
}
