using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Interactable{

    [SerializeField] protected List<Item> ContainedItems = new List<Item>();

    //Variables
    private string StorageName;

    //Sets it's own delegation
    private void Start(){
        Interaction = OpenContainer;

        //Sets the name
        StorageName = gameObject.name;

        if (Locked)
            gameObject.name = StorageName + " [Locked]";
        else 
            gameObject.name = StorageName;
        
    }

    //Opens the container for the player
    public void OpenContainer(CharacterFunctions stats) {
        stats.OpenContainer(ref ContainedItems, gameObject.name);
    }

}
