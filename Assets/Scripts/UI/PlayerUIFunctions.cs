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
        CloseOptions();

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
    public Text CarryWeight {
        get => _carryWeight != null ? _carryWeight :_carryWeight = transform.Find("Inventory/CarryWeight").GetComponent<Text>();
    }
    public GameObject OptionsMenu {
        get => _optionsMenu != null ? _optionsMenu : _optionsMenu = transform.Find("Inventory/Options").gameObject;
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

    //Variables
    float weightCarrying = 0;
    int clicked = 404;

    //Inventory Functions
    public void HideInventory(){
        InventoryHolder.gameObject.SetActive(false);
        CloseOptions();
    }
    public void OpenInventory(List<Item> newInventory)
    {
        UpdateInventory(newInventory);
        InventoryHolder.gameObject.SetActive(true);
    }
    public void ClickItem(int id){
        //Closes the options if the same button is pressed twice
        if (id != clicked){
            clicked = id;
            OpenOptions(id);
        }
        else
        {
            clicked = 404;
            CloseOptions();
        }
    }
    private void OpenOptions(int id) {
        //Sets the position before opening
        var newPos = InventorySlots[id].GetComponent<RectTransform>().anchoredPosition;
        OptionsMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(newPos.x + 80, newPos.y - 200);

        //Shows the options menu
        OptionsMenu.SetActive(true);
    }
    private void CloseOptions() {
        OptionsMenu.SetActive(false);
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

        //Sets the carry weight to 0
        weightCarrying = 0;

        for (int i = 0; i < InventorySlots.Count; i++) {
            //Updates the slots 
            if (newInventory.Count > i) {
                //Turns on the button and sets the reference
                InventorySlots[i].GetComponent<Button>().interactable = true;
                InventorySlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
                int num = i;
                InventorySlots[i].GetComponent<Button>().onClick.AddListener(delegate { ClickItem(num); });

                //Fixes image
                InventorySlots[i].transform.Find("Icon").GetComponent<Image>().enabled = true;
                InventorySlots[i].transform.Find("Icon").GetComponent<Image>().sprite = FindImage(newInventory[i].Name);

                //Counts the amount of weight that is being carried
                weightCarrying += newInventory[i].Weight;

                //Fixes Quantity
                InventorySlots[i].transform.Find("Quantity").GetComponent<Text>().text = newInventory[i].Quantity.ToString();

                continue;
            }

            //Creates an empty slot
            InventorySlots[i].transform.Find("Icon").GetComponent<Image>().enabled = false;
            InventorySlots[i].transform.Find("Quantity").GetComponent<Text>().text = "";
            InventorySlots[i].GetComponent<Button>().interactable = false;
        }

        //Fixes the weight carrying display
        CarryWeight.text = "Weight: " + weightCarrying.ToString("0.00");
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
