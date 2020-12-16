using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Interactable{

    [SerializeField] protected List<Item> ContainedItems = new List<Item>();

    //Variables
    private string StorageName;

    //Sets it's own delegation
    private void Start(){
        //Sets delegation
        Interaction = OpenContainer;

        //Sets the name
        StorageName = gameObject.name;

        //Fills in empty items
        for (int i = ContainedItems.Count; i < 15; i++)
            ContainedItems.Add(new Item());

        //Changes name to see if it's locked or not
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
