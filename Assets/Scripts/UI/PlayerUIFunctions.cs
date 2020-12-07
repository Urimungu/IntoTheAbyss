using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class PlayerUIFunctions : PlayerUIStats{

    //Initialization
    private void Start(){
        //Initializes the UI
        HideCrossHairText();
        InitializeInventory();
        HideInventory();

        //Checks to make sure there is a game manager and sets itself inside of it
        if (GameManager.Manager == null) return;

        //Destroys itself if there the reference is already set
        if (GameManager.Manager.PlayerUI != null) {
            Destroy(gameObject);
            return;
        }

        //Sets itself in the Game Manager
        GameManager.Manager.PlayerUI = this;
    }

    //Cross Hair
    public Image CrossHairIcon{
        get => _crossHairIcon != null ? _crossHairIcon : _crossHairIcon = transform.Find("CrossHair/Image").GetComponent<Image>();
    }
    public GameObject ItemDisplay {
        get => _itemDisplay != null ? _itemDisplay : _itemDisplay = transform.Find("ItemDisplay").gameObject;
    }
    public Text ItemDisplayText {
        get => _itemDisplayText != null ? _itemDisplayText : _itemDisplayText = transform.Find("ItemDisplay/Text").GetComponent<Text>();
    }

    //Inventory
    public List<GameObject> InventorySlots {
        get => _inventorySlots;
        set => _inventorySlots = value;
    }
    public Transform InventoryHolder {
        get => _inventoryHolder != null ? _inventoryHolder : _inventoryHolder = transform.Find("Inventory");
    }
    public Transform ItemHolder {
        get => _itemHolder != null ? _itemHolder : _itemHolder = transform.Find("Inventory/Items");
    }

    //Functions
    public void UpdateCrossHairText(string text) {
        //Shows the indicator if it was set to off
        ItemDisplay.SetActive(true);

        //Sets the text on the item
        ItemDisplayText.text = text;
    }
    public void HideCrossHairText() {
        //Turns off the display of the image
        ItemDisplay.SetActive(false);
    }
    public void UpdateCrossHairImage() {
        //Updates how the crosshair looks
    }

    //Inventory Functions
    public void HideInventory()
    {
        InventoryHolder.gameObject.SetActive(false);
    }
    public void OpenInventory(List<Item> newInventory)
    {
        UpdateInventory(newInventory);
        InventoryHolder.gameObject.SetActive(true);
    }
    private void InitializeInventory (){

        //Initializes variables
        var slot = Resources.Load<GameObject>("UI/Inventory/Prefabs/Slot");

        //Creates the inventory slots
        for (int y = 0; y < 7; y++){
            //Places the slots
            for (int x = 0; x < 5; x++){
                var obj = Instantiate(slot, ItemHolder);
                obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(10 + (110 * x), -10 - (110 * y));
                InventorySlots.Add(obj);
            }
        }
    }
    private void UpdateInventory(List<Item> newInventory){
        for (int i = 0; i < InventorySlots.Count; i++) {
            //Updates the slots 
            if (newInventory.Count > i) {
                //Fixes image
                InventorySlots[i].transform.Find("Icon").GetComponent<Image>().enabled = true;
                InventorySlots[i].transform.Find("Icon").GetComponent<Image>().sprite = FindImage(newInventory[i].Name);

                //Fixes Quantity
                InventorySlots[i].transform.Find("Quantity").GetComponent<Text>().text = newInventory[i].Quantity.ToString();

                continue;
            }

            //Creates an empty slot
            InventorySlots[i].transform.Find("Icon").GetComponent<Image>().enabled = false;
            InventorySlots[i].transform.Find("Quantity").GetComponent<Text>().text = "";
        }
    }
    private Sprite FindImage(string name) {
        //Checks to see if the sprite exists in the resource folder
        var newSprite = Resources.Load<Sprite>("UI/Inventory/Icons/" + name);
        if (newSprite != null)
            return newSprite;

        //Returns the blank one if the icon doesn't exist
        return Resources.Load<Sprite>("UI/Inventory/Icons/_NotFound");
    }
}
