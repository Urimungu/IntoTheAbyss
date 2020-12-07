using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUIStats : MonoBehaviour { 

    [Header("CrossHair")]
    [SerializeField] protected Image _crossHairIcon;
    [SerializeField] protected GameObject _itemDisplay;
    [SerializeField] protected Text _itemDisplayText;

    [Header("Inventory")]
    [SerializeField] protected List<GameObject> _inventorySlots = new List<GameObject>();
    [SerializeField] protected Transform _inventoryHolder;
    [SerializeField] protected Transform _itemHolder;

}
