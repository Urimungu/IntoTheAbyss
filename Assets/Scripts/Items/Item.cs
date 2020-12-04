using UnityEngine;

[System.Serializable]
public class Item{

    [Header("General Data")]
    public string Name;
    public string Desc;
    public int ID;

    //First Initialization
    public Item(string name, string desc, int id) {
        Name = name;
        Desc = desc;
        ID = id;
    }

}
