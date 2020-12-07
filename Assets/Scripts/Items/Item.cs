using UnityEngine;

[System.Serializable]
public class Item{

    [Header("General Data")]
    public string Name;
    public string Desc;
    public int ID;
    public int Quantity;

    //First Initialization
    public Item(string name, string desc, int id, int quantity) {
        Name = name;
        Desc = desc;
        ID = id;
        Quantity = quantity;
    }

}
