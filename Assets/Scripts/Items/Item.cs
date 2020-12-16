using UnityEngine;

[System.Serializable]
public class Item{

    [Header("General Data")]
    public string Name;
    public string Desc;
    public int ID;
    public int Quantity;
    public int MaxHoldCount;

    [Header("Physical Stats")]
    public float Weight;

    //First Initialization with general stats
    public Item(string name, string desc, int id, int quantity, int maxCount) {
        Name = name;
        Desc = desc;
        ID = id;
        Quantity = quantity;
        MaxHoldCount = maxCount;
    }

    //Creates empty Item
    public Item() { 
    }
}
